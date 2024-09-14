using BT.Runtime.Gameplay.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.General.Systems
{
    public sealed class BodyRotateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<ViewModelTransform> _bodyPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<ViewModelTransform>().End();
            _bodyPool = world.GetPool<ViewModelTransform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var body = ref _bodyPool.Get(ent);              
                        
                body.ModelTransformRef.rotation = Quaternion.Slerp
                (
                    body.ModelTransformRef.rotation,
                    body.LookAt,
                    Time.deltaTime * body.RotateSpeed
                );  
            }
        }
    }
}

