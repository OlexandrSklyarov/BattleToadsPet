using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;
using VContainer;

namespace BT.Runtime.Gameplay.Systems.Character
{
    public sealed class ChangeSpeedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;
        private HeroConfig.EngineConfig _config;

        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            _config = data.DIResolver.Resolve<MainConfig>().Hero.Engine;

            var world = systems.GetWorld();

            _filter = world.Filter<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .End();

            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var movement = ref  _movementDataPool.Get(ent);
                ref var input = ref  _inputDataPool.Get(ent);

                if (movement.IsGround && input.IsRun)
                {
                    movement.Speed *= _config.RunSpeedMultiplier;
                }
            }
        }
    }
}

