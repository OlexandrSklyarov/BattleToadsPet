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

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<MovementDataComponent>()
                .Inc<ViewModelTransformComponent>()
                .End();

            _bodyPool = world.GetPool<ViewModelTransformComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var body = ref _bodyPool.Get(ent); 
                ref var movement = ref  _movementDataPool.Get(ent);
                
                movement.Rotation = Vector3Math.DirToQuaternion(movement.Direction);
                body.ModelTransformRef.rotation = Quaternion.RotateTowards
                (
                    body.ModelTransformRef.rotation,
                    movement.Rotation,
                    Time.deltaTime * movement.RotateSpeed
                );   
            }
        }
    }
}

