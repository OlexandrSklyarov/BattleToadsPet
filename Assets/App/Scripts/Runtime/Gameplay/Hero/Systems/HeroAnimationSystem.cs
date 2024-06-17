using System;
using BT.Runtime.Data;
using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Extensions;
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

                //set speed prm
                var velMagnitude = new Vector3(movement.Velocity.x, 0f, movement.Velocity.z).magnitude;
                var normSpeed = Mathf.Clamp01(velMagnitude / config.ConfigRef.Engine.MaxRunSpeed);
                animator.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.NORM_SPEED_PRM, normSpeed);

                if (movement.IsGround) // land **************************************************************
                {
                    //simple
                    if (attack.IsExecuted)
                    {
                        if (IsState(ref animator, GameConstants.AnimatorPrm.ATTACK) && 
                            !IsStateTimeProgressHasReached(ref animator, config.ConfigRef.Attack.MinAttackAnimationProgress))
                        {
                            continue;
                        }

                        var item = config.ConfigRef.Attack.Combos[attack.ComboIndex];
                        PlayAttack(ref animator, item);

                        continue;   
                    }

                    //power
                    if (attack.IsExecutedPower)
                    {
                        if (IsState(ref animator, GameConstants.AnimatorPrm.ATTACK) &&
                            !IsStateTimeProgressHasReached(ref animator, config.ConfigRef.Attack.MinAttackAnimationProgress))
                        {
                            continue;
                        }

                        var item = config.ConfigRef.Attack.PowerAttackUp;
                        PlayAttack(ref animator, item);

                        continue;
                    }

                    //wait attack anim
                    if (IsState(ref animator, GameConstants.AnimatorPrm.ATTACK) && !IsStateTimeEnd(ref animator))
                    {
                        continue;
                    }
                     
                    //in case of falling and collision with the ground
                    if (IsState(ref animator, GameConstants.AnimatorPrm.FALL))
                    {                          
                        if (!IsState(ref animator, GameConstants.AnimatorPrm.JUMP_LANDING))
                        {
                            animator.AnimatorRef.Play(GameConstants.AnimatorPrm.JUMP_LANDING); 
                        }

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
                }
                else if (movement.IsJumping && movement.VerticalVelocity >= 0f)// Jump ******************************************************************************
                {
                    if (!IsState(ref animator, GameConstants.AnimatorPrm.JUMP))
                    {
                        animator.AnimatorRef.CrossFade
                        (
                            GameConstants.AnimatorPrm.JUMP, 
                            config.ConfigRef.Animation.CrosfadeAnimime
                        );
                    }                    

                    animator.IsPlayLocomotion = false;
                }
                else if (movement.IsJumping && movement.VerticalVelocity < 0f)// jump fall ******************************************************************************
                {
                    if (!IsState(ref animator, GameConstants.AnimatorPrm.FALL))
                    {
                        animator.AnimatorRef.CrossFade
                        (
                            GameConstants.AnimatorPrm.FALL, 
                            config.ConfigRef.Animation.CrosfadeAnimime
                        );
                    }                    

                    animator.IsPlayLocomotion = false;
                }
                else if (!movement.IsJumping && movement.VerticalVelocity <= -config.ConfigRef.Engine.MaxFallSpeed)// fall ******************************************************************************
                {
                    if (!IsState(ref animator, GameConstants.AnimatorPrm.FALL))
                    {
                        animator.AnimatorRef.CrossFade
                        (
                            GameConstants.AnimatorPrm.FALL, 
                            config.ConfigRef.Animation.CrosfadeAnimime
                        );
                    }                    

                    animator.IsPlayLocomotion = false;
                }
            }
        }

        private void PlayAttack(ref AnimatorComponent animator, ComboItem item)
        {
            animator.AnimatorRef.runtimeAnimatorController = item.AnimatorController;
            animator.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.ATTACK_SPEED_PRM, item.AnimationSpeed);
            animator.AnimatorRef.Play(GameConstants.AnimatorPrm.ATTACK);
        }

        private bool IsStateTimeProgressHasReached(ref AnimatorComponent animator, float normProgress)
        {
            return AnimatorExtensions.IsStateTimeProgressHasReached(animator.AnimatorRef,normProgress);
        }

        private bool IsStateTimeEnd(ref AnimatorComponent animator)
        {
            return AnimatorExtensions.IsStateTimeEnd(animator.AnimatorRef);
        }

        private bool IsState(ref AnimatorComponent animator, int state)
        {
            return AnimatorExtensions.IsState(animator.AnimatorRef, state);
        }
    }
}
