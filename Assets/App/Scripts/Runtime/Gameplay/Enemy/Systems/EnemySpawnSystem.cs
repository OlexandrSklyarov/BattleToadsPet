using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.Enemy.View;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Services.Spawn;
using Leopotam.EcsLite;
using UnityEngine;
using Util;
using VContainer;

namespace BT.Runtime.Gameplay.Enemy.Systems
{
    public sealed class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IItemGenerator _itemGenerator;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EnemySpawnerComponent> _enemySpawnerPool;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<HealthComponent> _healthPool;
        private EcsPool<IdleState> _idleStatePool;
        private EcsPool<Translate> _translatePool;
        private EcsPool<AnimatorController> _animatorPool;
        private EcsPool<ViewModelTransform> _viewModelTrPool;
        private EcsPool<NavMeshCharacterEngine> _navMeshEnginePool;
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
            _healthPool = _world.GetPool<HealthComponent>();
            _idleStatePool = _world.GetPool<IdleState>();
            _translatePool = _world.GetPool<Translate>();
            _animatorPool = _world.GetPool<AnimatorController>();
            _viewModelTrPool = _world.GetPool<ViewModelTransform>();
            _navMeshEnginePool = _world.GetPool<NavMeshCharacterEngine>();
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
            var enemyView = _itemGenerator.CreateEnemy(EnemyType.Drake, point);

            var entity = _world.NewEntity();

            AddEnemy(entity, enemyView);
            AddHealth(entity);
            AddIdleState(entity);
            AddTranslate(entity, enemyView);
            AddAnimator(entity, enemyView);
            AddBodyView(entity, enemyView);
            AddNavMeshEngine(entity, enemyView);

            //add entity collider
            RegisterEntityCollider(enemyView, entity);
        }

        private void AddNavMeshEngine(int entity, EnemyView enemyView)
        {
            _navMeshEnginePool.Add(entity).Ref = enemyView.NavMeshAgent;
        }

        private void AddBodyView(int entity, EnemyView enemyView)
        {
            _viewModelTrPool.Add(entity).ModelTransformRef = enemyView.ViewBody;
        }

        private void AddAnimator(int entity, EnemyView enemyView)
        {
            _animatorPool.Add(entity).AnimatorRef = enemyView.Animator;
        }

        private void AddTranslate(int entity, EnemyView enemyView)
        {
            _translatePool.Add(entity).Ref = enemyView.transform;
        }
        
        private void AddEnemy(int entity, EnemyView enemyView)
        {
            ref var comp = ref _enemyPool.Add(entity);
            comp.RotateBodySpeed = enemyView.Config.RotateBodySpeed;
            comp.AttackDistance = enemyView.Config.AttackDistance;
            comp.TriggerDistance = enemyView.Config.TriggerDistance;
            comp.MeeleAttackDelay = enemyView.Config.MeeleAttackDelay;
        }

        private void AddIdleState(int entity)
        {
            _idleStatePool.Add(entity);
        }

        private void AddHealth(int entity)
        {
            _healthPool.Add(entity).Value = 50;
        }

        private void RegisterEntityCollider(EnemyView enemyView, int entity)
        {
            if (!_entityColliders.ContainsKey(enemyView.Collider))
            {
                _entityColliders.Add(enemyView.Collider, _world.PackEntity(entity));
            }
        }
    }
}
