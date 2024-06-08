using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Services.Input;
using Leopotam.EcsLite;
using VContainer;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class HeroApplyInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<InputDataComponent> _inputDataPool;
        private IInputService _inputService;

        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            _inputService = data.DIResolver.Resolve<IInputService>();
           
            var world = systems.GetWorld();

            _filter = world.Filter<InputDataComponent>().End();
            _inputDataPool = world.GetPool<InputDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var input = ref  _inputDataPool.Get(ent);

                ResetInput(ref input);                   

                input.MoveDirection = _inputService.Movement;

                input.IsJumpWasPressed = _inputService.IsJumpWasPressed; 
                input.IsJumpHold = _inputService.IsJumpHold;    
                input.IsJumpWasReleased = _inputService.IsJumpWasReleased;    

                input.IsRunHold = _inputService.IsRunHold;  
                        
                input.IsAttackWasPressed = _inputService.IsAttackWasPressed;          
            }
        }

        private void ResetInput(ref InputDataComponent input)
        {
            input.IsJumpWasPressed = input.IsAttackWasPressed = input.IsRunHold = false; 
        }
    }
}
