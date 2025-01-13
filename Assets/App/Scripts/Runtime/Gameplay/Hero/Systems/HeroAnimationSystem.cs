using BT.Runtime.Data;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class HeroAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<AnimatorController> _animatorPool;
        private EcsPool<InputDataComponent> _inputPool;
        private EcsPool<MovementDataComponent> _movementPool;
        private EcsPool<CharacterConfigComponent> _characterConfigPool;
        private EcsPool<CharacterAttackComponent> _attackPool;
        private EcsPool<CharacterVelocity> _velocityPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<HeroTeg>()
                .Inc<AnimatorController>()
                .Inc<InputDataComponent>()
                .Inc<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<CharacterAttackComponent>()
                .Inc<CharacterVelocity>()
                .End();

            _animatorPool = world.GetPool<AnimatorController>();
            _inputPool = world.GetPool<InputDataComponent>();
            _movementPool = world.GetPool<MovementDataComponent>();
            _characterConfigPool = world.GetPool<CharacterConfigComponent>();
            _attackPool = world.GetPool<CharacterAttackComponent>();
            _velocityPool = world.GetPool<CharacterVelocity>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var animator = ref _animatorPool.Get(ent);
                ref var input = ref _inputPool.Get(ent);
                ref var movement = ref _movementPool.Get(ent);
                ref var config = ref _characterConfigPool.Get(ent);
                ref var attack = ref _attackPool.Get(ent);                
                ref var velocity = ref _velocityPool.Get(ent);                

                AnimationProcess(ref animator, ref velocity, ref movement, ref config, ref attack);
            }
        }

        private void AnimationProcess(
            ref AnimatorController animator, 
            ref CharacterVelocity velocity, 
            ref MovementDataComponent movement, 
            ref CharacterConfigComponent config, 
            ref CharacterAttackComponent attack)
        {
            SetMovementSpeedPrm(ref animator, ref velocity, ref config);          

            animator.Landed = movement.FallTime > config.ConfigRef.Animation.FallTimeThreshold && 
                movement.IsGroundFar;

            var state = GetMovementState(ref animator, ref movement, ref config);

            animator.Landed = false;

            if (state == animator.CurrentState) return;
            
            animator.AnimatorRef.CrossFade(state, config.ConfigRef.Animation.DefaultCrossFadeAnimation, 0);

            animator.CurrentState = state;
        }
      
        private void SetMovementSpeedPrm(ref AnimatorController animator, ref CharacterVelocity velocity, ref CharacterConfigComponent config)
        {
            var velMagnitude = new Vector3(velocity.Horizontal.x, 0f, velocity.Horizontal.z).magnitude;
            var normSpeed = Mathf.Clamp01(velMagnitude / config.ConfigRef.Engine.MaxRunSpeed);
            animator.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.NORM_SPEED_PRM, normSpeed);
        }

        private int GetMovementState(ref AnimatorController animator, ref MovementDataComponent movement, ref CharacterConfigComponent config)
        {
            var animConfig = config.ConfigRef.Animation;

            if (Time.time < animator.LockedTill) return animator.CurrentState;

            // Priorities
            if (animator.Landed && movement.IsGroundFar) return LockState(GameConstants.AnimatorPrm.LANDING, animConfig.LandingTime, ref animator);
            
            return (movement.IsGround || movement.IsGroundFar) ? GameConstants.AnimatorPrm.MOVEMENT : GameConstants.AnimatorPrm.FALL;

            int LockState(int s, float t, ref AnimatorController animator) 
            {
                animator.LockedTill = Time.time + t;
                return s;
            }
        }           
    }
}