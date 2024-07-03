using System.Diagnostics;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterJumpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;
        private EcsPool<CharacterVelocityComponent> _velocityPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .Inc<CharacterVelocityComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();
            _velocityPool = world.GetPool<CharacterVelocityComponent>();

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
                ref var config = ref _configPool.Get(e);   
                ref var velocity = ref _velocityPool.Get(e);   

                //only editor
                if (config.ConfigRef.IsChangePrmInRuntime)
                {
                    SetupGravityPrm(ref movement, ref config);
                }          
                
                Jump(ref movement, ref velocity, ref input);                
            }
        }

        private void Jump(ref MovementDataComponent movement, ref CharacterVelocityComponent velocity, ref InputDataComponent input)
        {
            movement.IsLanded = false;
            movement.IsStartJump = false;

            if (!movement.IsJumping && movement.IsGround && input.IsJumpHold)
            {
                movement.IsStartJump = true;
                movement.IsJumping = true;
                var previousVelocity = velocity.Vertical;
                var newVelocity = velocity.Vertical + movement.InitialJumpVelocity;
                var nextVelocity = (previousVelocity + newVelocity) * 0.5f;
                velocity.Vertical = nextVelocity;
            }
            else if (!input.IsJumpHold && movement.IsJumping && movement.IsGround)
            {
                movement.IsJumping = false;
                movement.IsLanded = true;
            }            
        }

        [Conditional("UNITY_EDITOR")]
        private void  SetupGravityPrm(ref MovementDataComponent movement, ref CharacterConfigComponent config)
        {
            var engineConfig = config.ConfigRef.Engine;

            var timeToApex = engineConfig.JumpTime / 2f;
            movement.Gravity = (-2f * engineConfig.MaxJumpHeight) / Mathf.Pow(timeToApex, 2f);
            movement.InitialJumpVelocity = (2f * engineConfig.MaxJumpHeight) / timeToApex; 
        }
    }
}

