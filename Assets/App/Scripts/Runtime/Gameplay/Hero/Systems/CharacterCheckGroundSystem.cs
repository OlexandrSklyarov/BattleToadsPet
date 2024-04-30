using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterCheckGroundSystem : IEcsInitSystem, IEcsRunSystem
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
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var engine = ref _characterEnginePool.Get(e); 

                movement.IsGround = engine.CharacterControllerRef.Controller.isGrounded;
            }
        }   
    }
}



