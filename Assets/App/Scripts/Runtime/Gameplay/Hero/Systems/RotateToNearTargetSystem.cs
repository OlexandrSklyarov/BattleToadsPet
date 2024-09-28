using System.Collections.Generic;
using System.Linq;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class RotateToNearTargetSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _sharedData;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<ViewModelTransform> _bodyPool;
        private EcsPool<CharacterVelocity> _velocityPool;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<Translate> _translatePool;
        private List<int> _entityList = new();

        public void Init(IEcsSystems systems)
        {
            _sharedData = systems.GetShared<SharedData>();
            
            _world = systems.GetWorld();

            _filter = _world.Filter<ViewModelTransform>()
                .Inc<CharacterVelocity>()
                .Inc<CharacterConfigComponent>()
                .End();

            _bodyPool = _world.GetPool<ViewModelTransform>();
            _velocityPool = _world.GetPool<CharacterVelocity>();
            _configPool = _world.GetPool<CharacterConfigComponent>();
            _enemyPool = _world.GetPool<EnemyComponent>();
            _translatePool = _world.GetPool<Translate>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var body = ref _bodyPool.Get(ent);
                ref var velocity = ref _velocityPool.Get(ent);
                ref var config = ref _configPool.Get(ent);

                if (velocity.Horizontal.magnitude < Mathf.Epsilon)
                {
                    if (TryFindNearEnemyPosition(ref body, config.ConfigRef.Attack.DetectTargetRadius, out Vector3 nearEnemyPosition))
                    {
                        body.LookAtDirection = nearEnemyPosition - body.ModelTransformRef.position;
                    }
                }
            }
        }

        private bool TryFindNearEnemyPosition(ref ViewModelTransform body, float radius, out Vector3 nearEnemyPosition)
        {
            nearEnemyPosition = default;

            var colliders = _sharedData.DetectTargetService.FindCollidersInRadius(body.ModelTransformRef.position, radius);  

            if (colliders.Any())
            {
                _entityList.Clear();

                foreach(var col in colliders)
                {
                    if (!_sharedData.EntityColliders.TryGetValue(col, out EcsPackedEntity target)) continue;
                    if (!target.Unpack(_world, out int entity)) continue;
                    if (!_enemyPool.Has(entity)) continue;
                    if (!_translatePool.Has(entity)) continue;

                    _entityList.Add(entity);               
                }

                var myPos = body.ModelTransformRef.position;
                var myForward = body.ModelTransformRef.forward;

                var nearEnemyTransform = _entityList
                    .Select(x => _translatePool.Get(x).Ref)
                    .OrderByDescending(x => Vector3.Dot(x.position - myPos, myForward))
                    .FirstOrDefault();
    
                if (nearEnemyTransform != null)
                {
                    nearEnemyPosition = nearEnemyTransform.position;
                    return true;
                }
            }

            return false;
        }
    }
}

