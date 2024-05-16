using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Services.Input;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class HeroApplyInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _heroPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;
        private IInputService _inputService;

        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            _inputService = data.DIResolver.Resolve<IInputService>();
           
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .End();

            _heroPool = world.GetPool<CharacterConfigComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var movement = ref _movementDataPool.Get(ent);
                ref var input = ref  _inputDataPool.Get(ent);
                ref var hero = ref  _heroPool.Get(ent);

                ResetInput(ref input);

                movement.RotateSpeed = hero.ConfigRef.Engine.RotateSpeed;
                movement.Speed = 0f;
                movement.MaxSpeed = hero.ConfigRef.Engine.MaxSpeed;

                if (_inputService.Movement.sqrMagnitude > Mathf.Epsilon)
                {
                    movement.Direction = new Vector3(_inputService.Movement.x, 0f, _inputService.Movement.y).normalized;
                    movement.Speed = hero.ConfigRef.Engine.MoveSpeed;
                }   

                input.IsJumpPressed = _inputService.IsJump;          
                input.IsAttack = _inputService.IsAttack;          
                input.IsRun = _inputService.IsRun;          
            }
        }

        private void ResetInput(ref InputDataComponent input)
        {
            input.IsJumpPressed = input.IsAttack = input.IsRun = false; 
        }
    }
}
