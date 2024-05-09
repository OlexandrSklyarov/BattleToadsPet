using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class HeroIKFootIKSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<CharacterFootIKComponent> _footIKPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<MovementDataComponent>()
                .Inc<CharacterFootIKComponent>()
                .End();

            _movementDataPool = world.GetPool<MovementDataComponent>();
            _footIKPool = world.GetPool<CharacterFootIKComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var ik = ref _footIKPool.Get(e);
                
                ik.FootIKRef.enabled = movement.IsGround;
            }
        }
    }
}

