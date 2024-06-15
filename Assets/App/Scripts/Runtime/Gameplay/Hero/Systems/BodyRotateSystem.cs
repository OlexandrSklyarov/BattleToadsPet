using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class BodyRotateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<ViewModelTransformComponent> _bodyPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<CharacterConfigComponent> _configDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<MovementDataComponent>()
                .Inc<ViewModelTransformComponent>()
                .Inc<CharacterConfigComponent>()
                .End();

            _bodyPool = world.GetPool<ViewModelTransformComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _configDataPool = world.GetPool<CharacterConfigComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var body = ref _bodyPool.Get(ent); 
                ref var movement = ref  _movementDataPool.Get(ent);
                ref var config = ref _configDataPool.Get(ent);

                var vel = movement.Velocity;
                vel.y = 0f;

                if (vel.sqrMagnitude < Mathf.Epsilon) return;

                movement.Rotation = Vector3Math.DirToQuaternion(movement.Velocity);            

                body.ModelTransformRef.rotation = Quaternion.Slerp
                (
                    body.ModelTransformRef.rotation,
                    movement.Rotation,
                    Time.deltaTime * config.ConfigRef.Engine.RotateSpeed
                );  
            }
        }
    }
}

