using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterCheckGroundSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterEngineComponent> _characterEnginePool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<CharacterConfigComponent> _characterConfigPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterEngineComponent>()
                .Inc<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .End();

            _characterEnginePool = world.GetPool<CharacterEngineComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _characterConfigPool = world.GetPool<CharacterConfigComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var engine = ref _characterEnginePool.Get(e); 
                ref var config = ref _characterConfigPool.Get(e); 

                movement.IsGround = Physics.CheckSphere
                (
                    engine.CharacterControllerRef.Controller.transform.TransformPoint(config.ConfigRef.Gravity.CheckGroundOffset),
                    config.ConfigRef.Gravity.CheckGroundSphereRadius,
                    config.ConfigRef.Gravity.GroundLayer
                );
                //movement.IsGround = engine.CharacterControllerRef.Controller.isGrounded;
            }
        }   
    }
}



