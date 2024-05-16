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
        
                if (config.ConfigRef.Engine.IsChangeRuntime)
                {
                    SetupGravityPrm(ref movement, ref config);
                }          
                
                if (!movement.IsJumping && movement.IsGround && input.IsJumpPressed) 
                {
                    movement.IsJumping = true;
                    movement.VerticalVelocity = movement.InitialJumpVelocity * 0.5f;
                }
                else if (!input.IsJumpPressed && movement.IsJumping && movement.IsGround)
                {             
                    movement.IsJumping = false;                    
                }
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void SetupGravityPrm(ref MovementDataComponent movement, ref CharacterConfigComponent config)
        {
            var timeToApex = config.ConfigRef.Engine.JumpTime / 2f;
            movement.Gravity = (-2f * config.ConfigRef.Engine.MaxJumpHeight) / Mathf.Pow(timeToApex, 2);
            movement.InitialJumpVelocity = (2f * config.ConfigRef.Engine.MaxJumpHeight) / timeToApex;
        }
    }
}

