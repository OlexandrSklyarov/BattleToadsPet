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
        private EcsPool<InputDataComponent> _inputDataPool;
        private IInputService _inputService;
        private HeroConfig _config;

        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            _inputService = data.DIResolver.Resolve<IInputService>();
            _config = data.DIResolver.Resolve<MainConfig>().Hero;
           
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
                ref var movement = ref _movementDataPool.Get(ent);
                ref var input = ref  _inputDataPool.Get(ent);

                ResetInput(ref input);

                movement.RotateSpeed = _config.Engine.RotateSpeed;
                movement.Speed = 0f;

                if (_inputService.Movement.sqrMagnitude > Mathf.Epsilon)
                {
                    movement.Direction = (new Vector3(_inputService.Movement.x, 0f, _inputService.Movement.y)).normalized;
                    movement.Speed = _config.Engine.MoveSpeed;
                }   

                input.IsJump = _inputService.IsJump;          
                input.IsAttack = _inputService.IsAttack;          
                input.IsRun = _inputService.IsRun;          
            }
        }

        private void ResetInput(ref InputDataComponent input)
        {
            input.IsJump = input.IsAttack = input.IsRun = false; 
        }
    }
}
