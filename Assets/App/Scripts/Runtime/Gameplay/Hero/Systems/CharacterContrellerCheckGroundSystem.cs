using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterContrellerCheckGroundSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterEngineComponent> _characterEnginePool;
        private EcsPool<MovementDataComponent> _movementPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterCheckGroundComponent>()
                .Inc<MovementDataComponent>()
                .End();

            _characterEnginePool = world.GetPool<CharacterEngineComponent>();
            _movementPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var engine = ref _characterEnginePool.Get(e); 
                ref var movement = ref _movementPool.Get(e);         

                movement.IsGround = engine.ControllerRef.CC.isGrounded;
            }
        }   
    }
}



