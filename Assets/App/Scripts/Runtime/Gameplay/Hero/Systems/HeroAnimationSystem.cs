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

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<HeroTeg>()
                .Inc<AnimatorComponent>()
                .Inc<InputDataComponent>()
                .Inc<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<CharacterAttackComponent>()
                .End();

            _animatorPool = world.GetPool<AnimatorComponent>();
            _inputPool = world.GetPool<InputDataComponent>();
            _movementPool = world.GetPool<MovementDataComponent>();
            _characterConfigPool = world.GetPool<CharacterConfigComponent>();
            _attackPool = world.GetPool<CharacterAttackComponent>();
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

                AnimationProcess(ref animator, ref movement, ref config, ref attack);
            }
        }

        private void AnimationProcess(ref AnimatorComponent animator, ref MovementDataComponent movement, ref CharacterConfigComponent config, ref CharacterAttackComponent attack)
        {
            SetMovementSpeedPrm(ref animator, ref movement, ref config);

            animator.Landed = movement.FallTime > 0.4f && movement.IsGroundFar;
            animator.Attacked = attack.LastAttackTime > 0f;

            var state = GetState(ref animator, ref movement, ref config, ref attack);

            animator.Landed = false;
            animator.Attacked = false;

            if (state == animator.CurrentState) return;
            
            animator.AnimatorRef.CrossFade(state, 0.1f, 0);

            if (state == GameConstants.AnimatorPrm.ATTACK) PlayAttackAnim(ref animator, ref attack, ref config);

            animator.CurrentState = state;
        }      

        private void SetMovementSpeedPrm(ref AnimatorComponent animator, ref MovementDataComponent movement, ref CharacterConfigComponent config)
        {
            var velMagnitude = new Vector3(movement.Velocity.x, 0f, movement.Velocity.z).magnitude;
            var normSpeed = Mathf.Clamp01(velMagnitude / config.ConfigRef.Engine.MaxRunSpeed);
            animator.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.NORM_SPEED_PRM, normSpeed);
        }

        private int GetState(ref AnimatorComponent animator, ref MovementDataComponent movement, ref CharacterConfigComponent config, ref CharacterAttackComponent attack)
        {
            var animConfig = config.ConfigRef.Animation;

            if (Time.time < animator.LockedTill) return animator.CurrentState;

            // Priorities
            if (animator.Attacked) return LockState(GameConstants.AnimatorPrm.ATTACK, attack.LastAttackTime, ref animator);
            if (animator.Landed) return LockState(GameConstants.AnimatorPrm.LANDING, animConfig.State.LandingTime, ref animator);

            return (movement.IsGround || movement.IsGroundFar) ? GameConstants.AnimatorPrm.MOVEMENT : GameConstants.AnimatorPrm.FALL;

            int LockState(int s, float t, ref AnimatorComponent animator) 
            {
                animator.LockedTill = Time.time + t;
                return s;
            }
        }

        private void PlayAttackAnim(ref AnimatorComponent animator, ref CharacterAttackComponent attack, ref CharacterConfigComponent config)
        {
            var item = (attack.IsExecutedPower) ?  
                config.ConfigRef.Attack.PowerAttackUp :
                config.ConfigRef.Attack.Combos[attack.ComboIndex];
            
            animator.AnimatorRef.runtimeAnimatorController = item.AnimatorController;
            animator.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.ATTACK_SPEED_PRM, item.AnimationSpeed);
            animator.AnimatorRef.Play(GameConstants.AnimatorPrm.ATTACK);           
        }   
    }
}