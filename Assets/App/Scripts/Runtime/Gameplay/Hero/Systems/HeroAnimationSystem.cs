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
        private EcsPool<AnimatorComponent> _animatorPool;
        private EcsPool<InputDataComponent> _inputPool;
        private EcsPool<MovementDataComponent> _movementPool;
        private EcsPool<CharacterConfigComponent> _characterConfigPool;
        private EcsPool<CharacterAttackComponent> _attackPool;
        private EcsPool<CharacterVelocityComponent> _velocityPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<HeroTeg>()
                .Inc<AnimatorComponent>()
                .Inc<InputDataComponent>()
                .Inc<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<CharacterAttackComponent>()
                .Inc<CharacterVelocityComponent>()
                .End();

            _animatorPool = world.GetPool<AnimatorComponent>();
            _inputPool = world.GetPool<InputDataComponent>();
            _movementPool = world.GetPool<MovementDataComponent>();
            _characterConfigPool = world.GetPool<CharacterConfigComponent>();
            _attackPool = world.GetPool<CharacterAttackComponent>();
            _velocityPool = world.GetPool<CharacterVelocityComponent>();
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

        private void AnimationProcess(ref AnimatorComponent animator, ref CharacterVelocityComponent velocity, ref MovementDataComponent movement, ref CharacterConfigComponent config, ref CharacterAttackComponent attack)
        {
            SetMovementSpeedPrm(ref animator, ref velocity, ref config);
            SetAttackDelayPrm(ref attack, ref animator);

            animator.Landed = movement.FallTime > 0.4f && movement.IsGroundFar;

            var state = GetState(ref animator, ref movement, ref config, ref attack);

            animator.Landed = false;

            if (state == animator.CurrentState) return;
            
            animator.AnimatorRef.CrossFade(state, config.ConfigRef.Animation.CrosfadeAnimime, 0);

            animator.CurrentState = state;
        }

        private void SetAttackDelayPrm(ref CharacterAttackComponent attack, ref AnimatorComponent animator)
        {
            animator.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.ATTACK_DELAY, attack.AttackTimeout);
        }

        private void SetMovementSpeedPrm(ref AnimatorComponent animator, ref CharacterVelocityComponent velocity, ref CharacterConfigComponent config)
        {
            var velMagnitude = new Vector3(velocity.Horizontal.x, 0f, velocity.Horizontal.z).magnitude;
            var normSpeed = Mathf.Clamp01(velMagnitude / config.ConfigRef.Engine.MaxRunSpeed);
            animator.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.NORM_SPEED_PRM, normSpeed);
        }

        private int GetState(ref AnimatorComponent animator, ref MovementDataComponent movement, ref CharacterConfigComponent config, ref CharacterAttackComponent attack)
        {
            var animConfig = config.ConfigRef.Animation;

            if (Time.time < animator.LockedTill) return animator.CurrentState;

            // Priorities
            if (animator.Landed) return LockState(GameConstants.AnimatorPrm.LANDING, animConfig.State.LandingTime, ref animator);
            
            return (movement.IsGround || movement.IsGroundFar) ? GameConstants.AnimatorPrm.MOVEMENT : GameConstants.AnimatorPrm.FALL;

            int LockState(int s, float t, ref AnimatorComponent animator) 
            {
                animator.LockedTill = Time.time + t;
                return s;
            }
        }           
    }
}