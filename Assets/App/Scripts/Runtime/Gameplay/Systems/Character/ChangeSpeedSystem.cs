using BT.Runtime.Gameplay.Components;
using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Systems.Character
{
    public sealed class ChangeSpeedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var movement = ref  _movementDataPool.Get(ent);
                ref var input = ref  _inputDataPool.Get(ent);
                ref var config = ref  _configPool.Get(ent);

                if (movement.IsGround && input.IsRun)
                {
                    movement.Speed *= config.ConfigRef.Engine.RunSpeedMultiplier;
                }
            }
        }
    }
}

