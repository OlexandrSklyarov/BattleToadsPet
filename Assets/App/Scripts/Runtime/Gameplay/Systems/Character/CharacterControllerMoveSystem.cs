using BT.Runtime.Gameplay.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Systems.Character
{
    public sealed class CharacterControllerMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterControllerComponent> _ccPool;
        private EcsPool<MovementDataComponent> _movementDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterControllerComponent>()
                .Inc<MovementDataComponent>()
                .End();

            _ccPool = world.GetPool<CharacterControllerComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var cc = ref  _ccPool.Get(ent);
                ref var movement = ref  _movementDataPool.Get(ent);

                var force = movement.Direction * movement.Speed * Time.deltaTime;
                cc.CCRef.Controller.Move(force);
            }
        }
    }
}
