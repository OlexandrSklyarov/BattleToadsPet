using BT.Runtime.Gameplay.General.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.General.Systems
{
    public sealed class MovementCharacterControllerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterControllerEngine> _characterEnginePool;
        private EcsPool<CharacterVelocity> _velocityPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterControllerEngine>()
                .Inc<CharacterVelocity>()
                .End();

            _characterEnginePool = world.GetPool<CharacterControllerEngine>();
            _velocityPool = world.GetPool<CharacterVelocity>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var engine = ref _characterEnginePool.Get(ent);                
                ref var velocity = ref _velocityPool.Get(ent);  
                
                engine.Ref.CC.Move(
                    new Vector3(velocity.Horizontal.x, velocity.Vertical, velocity.Horizontal.z) * Time.deltaTime);
            }
        }
    }
}
