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

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<HeroTeg>()
                .Inc<AnimatorComponent>()
                .Inc<InputDataComponent>()
                .Inc<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .End();

            _animatorPool = world.GetPool<AnimatorComponent>();
            _inputPool = world.GetPool<InputDataComponent>();
            _movementPool = world.GetPool<MovementDataComponent>();
            _characterConfigPool = world.GetPool<CharacterConfigComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var animator = ref _animatorPool.Get(ent);  
                ref var input = ref _inputPool.Get(ent);  
                ref var movement = ref _movementPool.Get(ent);   
                ref var config = ref _characterConfigPool.Get(ent);   

                //set speed prm
                var normSpeed = Mathf.Clamp01(movement.DesiredSpeed / movement.MaxSpeed);
                animator.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.NORM_SPEED_PRM, normSpeed);

                if (movement.IsGround) // land **************************************************************
                {
                    //in case of falling and collision with the ground
                    if (IsState(ref animator, GameConstants.AnimatorPrm.JUMP_FALL))
                    {                           
                        animator.AnimatorRef.Play(GameConstants.AnimatorPrm.JUMP_LANDING);                        
                        continue;
                    }

                    //if we lose the landing and the animation is not completed
                    if (IsState(ref animator, GameConstants.AnimatorPrm.JUMP_LANDING) && !IsStateTimeEnd(ref animator))
                    {
                        continue;
                    }

                    if (!animator.IsPlayLocomotion)
                    {
                        animator.AnimatorRef.CrossFade
                        (
                            GameConstants.AnimatorPrm.MOVEMENT, 
                            config.ConfigRef.Animation.CrosfadeAnimime
                        
                        );
                        animator.IsPlayLocomotion = true;
                        continue;
                    }

                    //default locomotion...
                    //animator.AnimatorRef.Play(GameConstants.AnimatorPrm.MOVEMENT);
                }
                else // fall ******************************************************************************
                {
                    if (movement.VerticalVelocity > 0f || 
                        movement.VerticalVelocity < -config.ConfigRef.Animation.MaxFallVelocity)
                    {
                        if (!IsState(ref animator, GameConstants.AnimatorPrm.JUMP_FALL))
                        {
                            animator.AnimatorRef.CrossFade
                            (
                                GameConstants.AnimatorPrm.JUMP_FALL, 
                                config.ConfigRef.Animation.CrosfadeAnimime
                            );
                        }
                    }

                    animator.IsPlayLocomotion = false;
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
