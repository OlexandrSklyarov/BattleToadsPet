using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.General.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.General.Systems
{
    public sealed class ClampWorldMovementOrientationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _modeFilter;
        private EcsFilter _bodyFilter;
        private EcsPool<GameWorldMovementOrientationComponent> _modePool;
        private EcsPool<ViewModelTransform> _bodyPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _modeFilter = world.Filter<GameWorldMovementOrientationComponent>().End();
            _bodyFilter = world.Filter<ViewModelTransform>().End();

            _modePool = world.GetPool<GameWorldMovementOrientationComponent>();
            _bodyPool = world.GetPool<ViewModelTransform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var eMode in _modeFilter)
            {
                ref var mode = ref _modePool.Get(eMode);  

                if (!mode.Is2DModeEnable) continue; // ignore 2D mode

                foreach (var ent in _bodyFilter)
                {
                    ref var body = ref _bodyPool.Get(ent);   

                    if (body.LookAtDirection.x >= 0f) //right
                    {
                        body.LookAtDirection = Vector3.right;
                    } 
                    else //left
                    {
                        body.LookAtDirection = Vector3.left;
                    }                      
                }
            }
        }
    }
}

