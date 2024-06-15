using System;
using System.Diagnostics;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterJumpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<CharacterEngineComponent> _characterEnginePool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _characterEnginePool = world.GetPool<CharacterEngineComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();

            foreach (var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var config = ref _configPool.Get(e);
                SetupGravityPrm(ref movement, ref config);
            }
        }        

        public void Run(IEcsSystems systems)
        {            
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var input = ref _inputDataPool.Get(e);
                ref var engine = ref _characterEnginePool.Get(e);
                ref var config = ref _configPool.Get(e);   

                //only editor
                if (config.ConfigRef.IsChangePrmInRuntime)
                {
                    SetupGravityPrm(ref movement, ref config);
                }          
                
                Jump(ref movement, ref config);

                engine.ControllerRef.RB.velocity = new Vector3
                (
                    engine.ControllerRef.RB.velocity.x, 
                    movement.VerticalVelocity, 
                    engine.ControllerRef.RB.velocity.z 
                );
            }
        }

        private void Jump(ref MovementDataComponent movement, ref CharacterConfigComponent config)
        {
            var enfineConfig = config.ConfigRef.Engine;

            if (movement.IsJumping)
            {
                if (movement.IsBumpedHead)
                {
                    movement.IsFastFalling = true;
                }

                if (movement.VerticalVelocity >= 0f)
                {
                    //apex controls
                    movement.ApexPoint = Mathf.InverseLerp(enfineConfig.InitialJumpVelocity, 0f, movement.VerticalVelocity);

                    if (movement.ApexPoint > enfineConfig.ApexThreshold)
                    {
                        if (!movement.IsPastApexThreshold)
                        {
                            movement.IsPastApexThreshold = true;
                            movement.TimePastApexThreshold = 0f;
                        }

                        if (movement.IsPastApexThreshold)
                        {
                            movement.TimePastApexThreshold += Time.fixedDeltaTime;
                            movement.VerticalVelocity = (movement.TimePastApexThreshold < enfineConfig.ApexHandTime) ? 0f : -0.01f;
                        }
                    }
                    // gravity on ascending *************************************************************
                    else
                    {
                        movement.VerticalVelocity += movement.Gravity * Time.fixedDeltaTime;
                        movement.IsPastApexThreshold = false;
                    }
                }
                // gravity on descending ***************************************************************
                else if (!movement.IsFastFalling)
                {
                    movement.VerticalVelocity += movement.Gravity * enfineConfig.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
                else if (movement.VerticalVelocity <= 0f)
                {
                    movement.IsFalling = true;
                }
            }

            //jump cut ******************************************************
            if (movement.IsFastFalling)
            {
                if (movement.FastFallTime >= enfineConfig.TimeForUpwardsCancel)
                {
                    movement.VerticalVelocity += movement.Gravity * enfineConfig.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
                else
                {
                    movement.VerticalVelocity = Mathf.Lerp
                    (
                        movement.FastFallReleasedSpeed,
                        0f,
                        movement.FastFallTime / enfineConfig.TimeForUpwardsCancel
                    );
                }

                movement.FastFallTime += Time.fixedDeltaTime;
            }

            //normal gravity *********************************************
            if (!movement.IsGround && !movement.IsJumping)
            {
                movement.IsFalling = true;
                movement.VerticalVelocity += movement.Gravity * Time.fixedDeltaTime;
            }

            //clamp fall speed *********************************************
            movement.VerticalVelocity = Mathf.Clamp(movement.VerticalVelocity, -enfineConfig.MaxFallSpeed, enfineConfig.MaxUpSpeed);
        }

        [Conditional("UNITY_EDITOR")]
        private void  SetupGravityPrm(ref MovementDataComponent movement, ref CharacterConfigComponent config)
        {
            var engineConfig = config.ConfigRef.Engine;

            movement.AdjustedJumpHeight = engineConfig.JumpHeight * engineConfig.JumpHeightCompensationFactor;
            movement.Gravity = -(2f * movement.AdjustedJumpHeight) / Mathf.Pow(engineConfig.TimeTillJumpApex, 2f);
            movement.InitialJumpVelocity = Mathf.Abs(movement.Gravity) * engineConfig.TimeTillJumpApex;
        }
    }
}

