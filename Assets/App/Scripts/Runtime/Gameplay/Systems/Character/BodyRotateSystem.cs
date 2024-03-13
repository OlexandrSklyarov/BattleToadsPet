using BT.Runtime.Gameplay.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Systems.Character
{
    public sealed class BodyRotateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<BodyTransformComponent> _bodyPool;
        private EcsPool<MovementDataComponent> _movementDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<MovementDataComponent>()
                .Inc<BodyTransformComponent>()
                .End();

            _bodyPool = world.GetPool<BodyTransformComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var body = ref _bodyPool.Get(ent); 
                ref var movement = ref  _movementDataPool.Get(ent);

                if (movement.Direction.sqrMagnitude > Mathf.Epsilon)
                {
                    movement.Rotation = Vector3Math.DirToQuaternion(movement.Direction);
                    body.BodyTrRef.rotation = Quaternion.RotateTowards
                    (
                        body.BodyTrRef.rotation,
                        movement.Rotation,
                        Time.deltaTime * 360f
                    );
                }

            }
        }
    }
}

