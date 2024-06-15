using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterCheckBumpedHeadSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterCheckGroundComponent> _characterGroundPool;
        private EcsPool<MovementDataComponent> _movementPool;
        private EcsPool<CharacterConfigComponent> _characterConfigPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterCheckGroundComponent>()
                .Inc<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .End();

            _characterGroundPool = world.GetPool<CharacterCheckGroundComponent>();
            _movementPool = world.GetPool<MovementDataComponent>();
            _characterConfigPool = world.GetPool<CharacterConfigComponent>();;
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var e in _filter)
            {
                ref var ground = ref _characterGroundPool.Get(e); 
                ref var movement = ref _movementPool.Get(e); 
                ref var config = ref _characterConfigPool.Get(e); 

                var boxCastOrigin = new Vector3
                (
                    ground.FeetCollider.bounds.center.x,
                    ground.BodyCollider.bounds.max.y,
                    ground.FeetCollider.bounds.center.z
                );

                var boxCastSize = new Vector3
                (
                    ground.FeetCollider.bounds.size.x * config.ConfigRef.Gravity.HeadWidth,
                    config.ConfigRef.Gravity.HeadDetectionRayLength,
                    ground.FeetCollider.bounds.size.z * config.ConfigRef.Gravity.HeadWidth
                );     

                var count = Physics.OverlapBoxNonAlloc
                (
                    boxCastOrigin,
                    boxCastSize,
                    ground.HeadBumpResult,
                    Quaternion.identity,
                    config.ConfigRef.Gravity.GroundLayer
                );          

                movement.IsBumpedHead = count > 0;    
            }
        }   
    }
}



