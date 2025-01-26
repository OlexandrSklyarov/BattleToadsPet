using BT.Runtime.Gameplay.Enemy.View;
using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;

namespace BT.Runtime.Services.Spawn
{
    public interface IItemGenerator
    {
        T SpawnPrefab<T>(T prefab, Transform parent) where T : MonoBehaviour;
        HeroView GetHero(HeroType type, Transform spawnPoint);
        EnemyView CreateEnemy(EnemyType type, Vector3 spawnPoint);
    } 
}


