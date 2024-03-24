using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace BT.Runtime.Gameplay.Systems.Character
{
    public sealed class CharacterJumpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterControllerComponent> _characterPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;
        private HeroConfig.EngineConfig _config;

        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            _config = data.DIResolver.Resolve<MainConfig>().Hero.Engine;

            var world = systems.GetWorld();

            _filter = world.Filter<CharacterControllerComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .End();

            _characterPool = world.GetPool<CharacterControllerComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var input = ref _inputDataPool.Get(e);
                ref var character = ref _characterPool.Get(e);
                
                if (movement.IsGround && input.IsJump) 
                {
                    var jumpForce = _config.JumpForce;
                    movement.VerticalVelocity = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
                    character.CCRef.Controller.Move(Vector3.up * movement.VerticalVelocity * Time.deltaTime);
                }
            }
        }
    }
}

