using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterGravitySystem : IEcsInitSystem, IEcsRunSystem
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
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var engine = ref _characterEnginePool.Get(e); 
                ref var config = ref _configPool.Get(e); 
                ref var input = ref _inputDataPool.Get(e); 

                // movement.IsFalling = movement.Velocity.y <= 0f || !input.IsJumpWasPressed;

                // if (movement.IsGround)
                // {
                //     movement.Velocity.y = -config.ConfigRef.Gravity.MinVerticalVelocity;
                // }
                // else if (movement.IsFalling)
                // {
                //     var prevVelocity = movement.Velocity.y;
                //     var newVelocity = movement.Velocity.y + (movement.Gravity * config.ConfigRef.Gravity.FallMultiplier * Time.deltaTime);
                //     var nextVelocity = Mathf.Max((prevVelocity + newVelocity) * 0.5f, -config.ConfigRef.Gravity.MaxFallVelocity);
                //     movement.Velocity.y = nextVelocity;
                // }
                // else
                // {
                //     var prevVelocity = movement.Velocity.y;
                //     var newVelocity = movement.Velocity.y + (movement.Gravity * Time.deltaTime);
                //     var nextVelocity = (prevVelocity + newVelocity) * 0.5f;
                //     movement.Velocity.y = nextVelocity;
                // }
            }
        } 
    }
}


