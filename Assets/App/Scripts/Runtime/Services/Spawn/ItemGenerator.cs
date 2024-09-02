using BT.Runtime.Gameplay.Enemy.View;
using BT.Runtime.Gameplay.Views.Hero;
using BT.Runtime.Services.Spawn.Factory;
using UnityEngine;

namespace BT.Runtime.Services.Spawn
{
    public sealed class ItemGenerator : IItemGenerator
    {
        private ICharacterFactory _characterFactory;

        public ItemGenerator(ICharacterFactory characterFactory)
        {
            _characterFactory = characterFactory;
        }

        public EnemyView CreateEnemy(EnemyType type, Vector3 spawnPoint)
        {
            return _characterFactory.CreateEnemy(type, spawnPoint);
        }

        public T SpawnPrefab<T>(T prefab, Transform spawnPoint) where T : MonoBehaviour
        {
            return UnityEngine.Object.Instantiate(prefab, spawnPoint);
        }

        HeroView IItemGenerator.GetHero(HeroType type, Transform spawnPoint)
        {
            return _characterFactory.GetHero(type, spawnPoint);
        }
    }
}

