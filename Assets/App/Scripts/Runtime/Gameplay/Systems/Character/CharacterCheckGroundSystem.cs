using BT.Runtime.Gameplay.Components;
using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Systems.Character
{
    public sealed class CharacterCheckGroundSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterControllerComponent> _characterPool;
        private EcsPool<MovementDataComponent> _movementDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterControllerComponent>()
                .Inc<MovementDataComponent>()
                .End();

            _characterPool = world.GetPool<CharacterControllerComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var character = ref _characterPool.Get(e); 

                movement.IsGround = character.CCRef.Controller.isGrounded;
            }
        }   
    }
}



