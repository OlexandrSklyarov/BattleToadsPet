using System.Collections.Generic;
using BT.Runtime.Gameplay.Characters.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Services.Spawn;
using Leopotam.EcsLite;
using UnityEngine;
using Util;
using VContainer;

namespace BT.Runtime.Gameplay.Characters.Systems
{
    public sealed class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IItemGenerator _itemGenerator;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EnemySpawnerComponent> _enemySpawnerPool;
        private EcsPool<EnemyComponent> _enemyPool;
        private Dictionary<Collider, EcsPackedEntity> _entityColliders;

        public void Init(IEcsSystems systems)
        {
            _entityColliders = systems.GetShared<SharedData>().EntityColliders;
            var resolver = systems.GetShared<SharedData>().DIResolver;
            _itemGenerator = resolver.Resolve<IItemGenerator>();
            
            _world = systems.GetWorld();
            _filter = _world.Filter<EnemySpawnerComponent>().End();
            _enemySpawnerPool = _world.GetPool<EnemySpawnerComponent>();
            _enemyPool = _world.GetPool<EnemyComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            if (_enemyPool.GetRawDenseItemsCount() > 5) return;
            
            foreach(var ent in _filter)
            {
                ref var spawner = ref  _enemySpawnerPool.Get(ent);

                if (spawner.NextSpawnTimer > 0f)
                {
                    spawner.NextSpawnTimer -= Time.deltaTime;
                    return;
                }

                spawner.NextSpawnTimer = 5f;
                SpawnEnemy(spawner.Points.RandomElement());
            }
        }

        private void SpawnEnemy(Vector3 point)
        {
            var enemyView = _itemGenerator.CreateEnemy(Views.EnemyType.Drake, point);

            var entity = _world.NewEntity();
            _enemyPool.Add(entity);

            //add entity collider
            if (!_entityColliders.ContainsKey(enemyView.Collider))
            {
                _entityColliders.Add(enemyView.Collider, _world.PackEntity(entity));
            }           
        }
    }
}
