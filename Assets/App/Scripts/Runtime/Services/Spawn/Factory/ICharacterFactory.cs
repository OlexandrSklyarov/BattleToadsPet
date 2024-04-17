using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;

namespace BT.Runtime.Services.Spawn.Factory
{
    public interface ICharacterFactory
    {
        HeroView GetHero(HeroType type, Transform spawnPoint);
    }
}

