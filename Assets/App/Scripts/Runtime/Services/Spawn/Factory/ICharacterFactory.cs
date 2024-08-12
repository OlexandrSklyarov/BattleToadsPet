using BT.Runtime.Gameplay.Characters.Views;
using BT.Runtime.Gameplay.View;
using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;

namespace BT.Runtime.Services.Spawn.Factory
{
    public interface ICharacterFactory
    {
        EnemyView CreateEnemy(EnemyType type, Vector3 spawnPoint);
        HeroView GetHero(HeroType type, Transform spawnPoint);
    }
}

