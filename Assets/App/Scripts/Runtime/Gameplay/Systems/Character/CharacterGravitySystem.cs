using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace BT.Runtime.Gameplay.Systems.Character
{
    public sealed class CharacterGravitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterControllerComponent> _characterPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private HeroConfig.EngineConfig _config;

        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            _config = data.DIResolver.Resolve<MainConfig>().Hero.Engine;

            var world = systems.GetWorld();

            _filter = world.Filter<CharacterControllerComponent>()
                .Inc<MovementDataComponent>()
                .End();

            _characterPool = world.GetPool<CharacterControllerComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var character = ref _characterPool.Get(e); 

                ClampVerticalVelocity(ref movement, _config.MinVerticalVelocity);
                
                var gravityMultiplier = (!movement.IsGround) ? _config.FallGravityMultiplier : 1f;

                movement.VerticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime; 
                movement.VerticalVelocity = Mathf.Max(Physics.gravity.y, movement.VerticalVelocity);
                
                character.CCRef.Controller.Move(Vector3.up * movement.VerticalVelocity * Time.deltaTime);
            }
        }       

        private void ClampVerticalVelocity(ref MovementDataComponent movement, float minVerticalVelocity)
        {
            if (!movement.IsGround) return;
            if (movement.VerticalVelocity >= 0f) return;
            
            movement.VerticalVelocity = -minVerticalVelocity;
        }
    }
}


