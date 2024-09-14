using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using Util;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class HeroSetLookSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<ViewModelTransform> _bodyPool;
        private EcsPool<CharacterVelocity> _velocityPool;
        private EcsPool<CharacterConfigComponent> _configPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<ViewModelTransform>()
                .Inc<CharacterVelocity>()
                .Inc<CharacterConfigComponent>()
                .End();

            _bodyPool = world.GetPool<ViewModelTransform>();
            _velocityPool = world.GetPool<CharacterVelocity>();
            _configPool = world.GetPool<CharacterConfigComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var body = ref _bodyPool.Get(ent);    
                ref var velocity = ref _velocityPool.Get(ent);
                ref var config = ref _configPool.Get(ent);

                body.RotateSpeed = config.ConfigRef.Engine.RotateSpeed;

                var vel = velocity.Horizontal;
                vel.y = 0f;

                if (vel.sqrMagnitude <= 0.001f) return;

                body.LookAt = Vector3Math.DirToQuaternion(velocity.Horizontal);
            }
        }
    }
}

