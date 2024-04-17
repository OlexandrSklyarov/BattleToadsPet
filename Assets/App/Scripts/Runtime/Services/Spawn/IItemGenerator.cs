using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;

namespace BT.Runtime.Services.Spawn
{
    public interface IItemGenerator
    {
        HeroView GetHero(HeroType type, Transform spawnPoint);
    } 
}


