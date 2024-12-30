using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterControllerCheckGroundSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterControllerEngine> _characterEnginePool;
        private EcsPool<MovementDataComponent> _movementPool;
        private EcsPool<CharacterConfigComponent> _configPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterCheckGroundComponent>()
                .Inc<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .End();

            _characterEnginePool = world.GetPool<CharacterControllerEngine>();
            _movementPool = world.GetPool<MovementDataComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var engine = ref _characterEnginePool.Get(e); 
                ref var movement = ref _movementPool.Get(e);         
                ref var config = ref _configPool.Get(e);         

                movement.IsGround = engine.Ref.CC.isGrounded;

                movement.IsGroundFar = Physics.Raycast
                (
                    new Ray(engine.Ref.CC.transform.position,Vector3.down),
                    config.ConfigRef.Gravity.GroundDetectionRayLength,
                    config.ConfigRef.Gravity.GroundLayer
                );                 
            }
        }   
    }
}



