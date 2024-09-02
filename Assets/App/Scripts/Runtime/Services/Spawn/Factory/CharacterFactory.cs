using System.Linq;
using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Enemy.View;
using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;

namespace BT.Runtime.Services.Spawn.Factory
{
    public sealed class CharacterFactory : ICharacterFactory
    {
        private readonly FactoryItemsConfig _config;

        public CharacterFactory(FactoryItemsConfig config)
        {
            _config = config;
        }

        public EnemyView CreateEnemy(EnemyType type, Vector3 spawnPoint)
        {
            return UnityEngine.Object.Instantiate
            (
                _config.Enemies.First(x => x.Type == type).Prefab, 
                spawnPoint,
                Quaternion.identity
            );
        }

        HeroView ICharacterFactory.GetHero(HeroType type, Transform spawnPoint)
        {
            return UnityEngine.Object.Instantiate
            (
                _config.Heroes.First(x => x.Type == type).HeroPrefab, 
                spawnPoint.position,
                spawnPoint.rotation
            );  
        }
    }
}
