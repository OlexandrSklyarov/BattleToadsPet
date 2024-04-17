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

        HeroView IItemGenerator.GetHero(HeroType type, Transform spawnPoint)
        {
            return _characterFactory.GetHero(type, spawnPoint);
        }
    }
}

