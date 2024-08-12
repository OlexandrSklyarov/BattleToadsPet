using BT.Runtime.Data.Configs;
using BT.Runtime.Data.Persistent;
using BT.Runtime.Gameplay.Characters.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Gameplay.Views.World;
using BT.Runtime.Services.Player;
using BT.Runtime.Services.Spawn;
using Leopotam.EcsLite;
using Util;
using VContainer;

namespace BT.Runtime.Gameplay.Map.Systems
{
    public sealed class SpawnMapSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var resolver = systems.GetShared<SharedData>().DIResolver;
            var levelDataBase = resolver.Resolve<MainConfig>().LevelDataBase;
            var storageService = resolver.Resolve<IPlayerDataStorageService>();
            var worldTR = resolver.Resolve<WorldTeg>().transform;            
            
            var index = storageService.GetData<LevelData>().NextLevelIndex;
            var prefab = levelDataBase.Levels[index].MapPrefab;

            var map = resolver.Resolve<IItemGenerator>().SpawnPrefab(prefab, worldTR);
            map.Init();

            //EnemySpawner *********************************************
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            ref var spawner = ref world.GetPool<EnemySpawnerComponent>().Add(entity);
            spawner.Points = map.EnemySpawnPoints; 
            spawner.NextSpawnTimer = 5f; 

            DebugUtil.Print(map.EnemySpawnPoints);   
            DebugUtil.Print(spawner.Points);   
        }
    }
}
