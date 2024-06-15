using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterJumpChecksSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();            
        }        

        public void Run(IEcsSystems systems)
        {            
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var input = ref _inputDataPool.Get(e);
                ref var config = ref _configPool.Get(e); 

                CountTimers(ref movement, ref config);      
                JumpChecks(ref input, ref movement, ref config);            
            }
        }

        private void JumpChecks(ref InputDataComponent input, 
            ref MovementDataComponent movement, 
            ref CharacterConfigComponent config)
        {
            var engineConfig = config.ConfigRef.Engine;

            // press jump **********************************************************
            if (input.IsJumpWasPressed)
            {
                movement.JumpBufferTimer = engineConfig.JumpBufferTime;
                movement.IsJumpReleasedDuringBuffer = false;
            }

            //release jump **********************************************************
            if (input.IsJumpWasReleased)
            {
                if (movement.JumpBufferTimer > 0f)
                {
                    movement.IsJumpReleasedDuringBuffer = true;
                }

                if (movement.IsJumping && movement.VerticalVelocity > 0f)
                {
                    if (movement.IsPastApexThreshold)
                    {
                        movement.IsPastApexThreshold = false;
                        movement.IsFastFalling = true;
                        movement.FastFallTime = engineConfig.TimeForUpwardsCancel;
                        movement.VerticalVelocity = 0f;
                    }
                    else
                    {
                        movement.IsFastFalling = true;
                        movement.FastFallReleasedSpeed = movement.VerticalVelocity;
                    }
                }
            }

            //init jump **********************************************************
            if (movement.JumpBufferTimer > 0f && !movement.IsJumping && 
                (movement.IsGround || movement.CoyoteTime > 0f))
            {
                InitialJump(ref movement, ref config, 1);

                if (movement.IsJumpReleasedDuringBuffer)
                {
                    movement.IsFastFalling = true;
                    movement.FastFallReleasedSpeed = movement.VerticalVelocity;
                }
            }
            //double jump **********************************************************
            else if (movement.JumpBufferTimer > 0f && movement.IsJumping && 
                movement.NumberOfJumpsUsed < engineConfig.NumberofJumpsAllowed)
            {
                movement.IsFastFalling = false;
                InitialJump(ref movement, ref config, 1);
            }
            //air jump **********************************************************
            else if (movement.JumpBufferTimer > 0f && movement.IsFalling && 
                movement.NumberOfJumpsUsed < engineConfig.NumberofJumpsAllowed - 1)
            {
                InitialJump(ref movement, ref config, 2);
                movement.IsFastFalling = false;
            }

            //landed **********************************************************
            if ((movement.IsJumping || movement.IsFalling) && 
                movement.IsGround && movement.VerticalVelocity <= 0f)
            {
                ReleasedJumpValues(ref movement);
            }
        }

        private void ReleasedJumpValues(ref MovementDataComponent movement)
        {
            movement.IsJumping = false;
            movement.IsFalling = false;
            movement.IsFastFalling = false;
            movement.IsPastApexThreshold = false;
            movement.FastFallTime = 0f;
            movement.NumberOfJumpsUsed = 0;
            movement.VerticalVelocity = Physics.gravity.y;
        }

        private void InitialJump(ref MovementDataComponent movement, ref CharacterConfigComponent config, int jumpNumValue)
        {
            if (!movement.IsJumping)
            {
                movement.IsJumping = true;
            }

            movement.JumpBufferTimer = 0f;
            movement.NumberOfJumpsUsed += jumpNumValue;
            movement.VerticalVelocity = config.ConfigRef.Engine.InitialJumpVelocity;
        }

        private void CountTimers(ref MovementDataComponent movement, ref CharacterConfigComponent config)
        {
            movement.JumpBufferTimer -= Time.deltaTime;

            if (!movement.IsGround)
            {
                movement.CoyoteTime -= Time.deltaTime;
            }
            else
            {
                movement.CoyoteTime = config.ConfigRef.Engine.JumpCoyoteTime;
            }

            //clamp
            movement.JumpBufferTimer = Mathf.Max(0f, movement.JumpBufferTimer);
            movement.CoyoteTime = Mathf.Max(0f, movement.CoyoteTime);
        }
    }
}

