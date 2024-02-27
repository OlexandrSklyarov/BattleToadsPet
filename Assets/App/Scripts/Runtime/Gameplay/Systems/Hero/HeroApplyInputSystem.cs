using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Services.Input;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace BT.Runtime.Gameplay.Systems.Hero
{
    public sealed class HeroApplyInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private IInputService _inputService;
        private HeroConfig _config;

        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            _inputService = data.DIResolver.Resolve<IInputService>();
            _config = data.DIResolver.Resolve<MainConfig>().Hero;
           
            var world = systems.GetWorld();

            _filter = world.Filter<MovementDataComponent>().End();
            _movementDataPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var movement = ref  _movementDataPool.Get(ent);

                movement.Direction = (new Vector3(_inputService.Movement.x, 0f, _inputService.Movement.y)).normalized;
                movement.Speed = _config.Engine.MoveSpeed;
            }
        }
    }
}
