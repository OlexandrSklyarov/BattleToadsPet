using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterControllerMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterEngineComponent> _characterEnginePool;
        private EcsPool<MovementDataComponent> _movementDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterEngineComponent>()
                .Inc<MovementDataComponent>()
                .End();

            _characterEnginePool = world.GetPool<CharacterEngineComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var engine = ref  _characterEnginePool.Get(ent);
                ref var movement = ref  _movementDataPool.Get(ent);

                var force = movement.Direction * movement.TargetSpeed * Time.deltaTime;
                engine.CharacterControllerRef.Controller.Move(force);
            }
        }
    }
}
