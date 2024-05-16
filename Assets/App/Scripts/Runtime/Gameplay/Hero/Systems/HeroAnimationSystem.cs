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

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<HeroTeg>()
                .Inc<AnimatorComponent>()
                .Inc<InputDataComponent>()
                .Inc<MovementDataComponent>()
                .End();

            _animatorPool = world.GetPool<AnimatorComponent>();
            _inputPool = world.GetPool<InputDataComponent>();
            _movementPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var animator = ref _animatorPool.Get(ent);  
                ref var input = ref _inputPool.Get(ent);  
                ref var movement = ref _movementPool.Get(ent);   

                var normSpeed = Mathf.Clamp01(movement.DesiredSpeed / movement.MaxSpeed);
                animator.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.NORM_SPEED_PRM, normSpeed);

                if (movement.IsGround)
                {
                    if (IsState(ref animator, GameConstants.AnimatorPrm.JUMP_FALL))
                    {
                        animator.AnimatorRef.Play(GameConstants.AnimatorPrm.JUMP_LANDING);
                        continue;
                    }

                    if (IsState(ref animator, GameConstants.AnimatorPrm.JUMP_LANDING) && !IsStateTimeEnd(ref animator))
                    {
                        continue;
                    }
                    
                    animator.AnimatorRef.Play(GameConstants.AnimatorPrm.MOVEMENT);
                }
                else
                {
                    if (movement.VerticalVelocity > 0f || movement.VerticalVelocity <= movement.Gravity)
                    {
                        animator.AnimatorRef.Play(GameConstants.AnimatorPrm.JUMP_FALL);
                    }
                }
            }
        }

        private bool IsStateTimeEnd(ref AnimatorComponent animator)
        {
            return animator.AnimatorRef.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }

        private bool IsState(ref AnimatorComponent animator, int state)
        {
            var info = animator.AnimatorRef.GetCurrentAnimatorStateInfo(0);
            return info.shortNameHash == state;
        }
    }
}
