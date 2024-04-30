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
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var input = ref _inputDataPool.Get(e);
                ref var engine = ref _characterEnginePool.Get(e);
                ref var config = ref _configPool.Get(e);
                
                if (movement.IsGround && input.IsJump) 
                {
                    var jumpForce = config.ConfigRef.Engine.JumpForce;
                    movement.VerticalVelocity = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
                    engine.CharacterControllerRef.Controller.Move(Vector3.up * movement.VerticalVelocity * Time.deltaTime);
                }
            }
        }
    }
}

