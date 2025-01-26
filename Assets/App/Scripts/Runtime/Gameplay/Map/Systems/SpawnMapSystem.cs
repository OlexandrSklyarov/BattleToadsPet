using System;
using BT.Runtime.Data.Configs;
using BT.Runtime.Data.Persistent;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Map.View;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Gameplay.Views.World;
using BT.Runtime.Services.Player;
using BT.Runtime.Services.Spawn;
using Leopotam.EcsLite;
using UnityEngine;
using Util;
using VContainer;

namespace BT.Runtime.Gameplay.Map.Systems
{
    public sealed class SpawnMapSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var map = InitMap(systems);

            CreateEnemySpawnerEntity(world, map);
            
            CreateHeroSpawnerEntity(world, map);

            CreateWorldMovementOrientation(world);
        }

        private MapViewMediator InitMap(IEcsSystems systems)
        {
            var resolver = systems.GetShared<SharedData>().DIResolver;
            var levelDataBase = resolver.Resolve<MainConfig>().LevelDataBase;
            var storageService = resolver.Resolve<IPlayerDataStorageService>();
            var worldTR = new GameObject("[WORLD]").transform;

            var index = storageService.GetData<LevelData>().NextLevelIndex;
            var prefab = levelDataBase.Levels[index].MapPrefab;

            var map = resolver.Resolve<IItemGenerator>().SpawnPrefab(prefab, worldTR);
            map.Init();

            return map;
        }

        private void CreateHeroSpawnerEntity(EcsWorld world, MapViewMediator map)
        {
            var entity = world.NewEntity();
            ref var spawner = ref world.GetPool<HeroSpawnerComponent>().Add(entity);
            spawner.SpawnPosition = map.GetComponentInChildren<SpawnPointTag>().transform;
        }

        private void CreateEnemySpawnerEntity(EcsWorld world, MapViewMediator map)
        {
            var entity = world.NewEntity();
            ref var spawner = ref world.GetPool<EnemySpawnerComponent>().Add(entity);
            spawner.Points = map.EnemySpawnPoints;
            spawner.NextSpawnTimer = 5f;          

            DebugUtil.Print(map.EnemySpawnPoints);
            DebugUtil.Print(spawner.Points);
        }

        private void CreateWorldMovementOrientation(EcsWorld world)
        {
            var entity = world.NewEntity();
            ref var orientation = ref world.GetPool<GameWorldMovementOrientationComponent>().Add(entity);
            //orientation.Is2DModeEnable = true;
        }
    }
}
