using BT.Runtime.Gameplay.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Systems.Character
{
    public sealed class CharacterGravitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<CharacterEngineComponent> _characterEnginePool;
        private EcsPool<MovementDataComponent> _movementDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<MovementDataComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _characterEnginePool = world.GetPool<CharacterEngineComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var engine = ref _characterEnginePool.Get(e); 
                ref var config = ref _configPool.Get(e); 

                ClampVerticalVelocity(ref movement, config.ConfigRef.Gravity.MinVerticalVelocity);
                
                var gravityMultiplier = (!movement.IsGround) ? config.ConfigRef.Gravity.FallGravityMultiplier : 1f;

                movement.VerticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime; 
                movement.VerticalVelocity = Mathf.Max(Physics.gravity.y, movement.VerticalVelocity);
                
                engine.CharacterControllerRef.Controller.Move(Vector3.up * movement.VerticalVelocity * Time.deltaTime);
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


