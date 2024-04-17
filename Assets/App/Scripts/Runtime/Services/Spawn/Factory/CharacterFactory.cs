using System.Linq;
using BT.Runtime.Data.Configs;
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

        HeroView ICharacterFactory.GetHero(HeroType type, Transform spawnPoint)
        {
            return UnityEngine.Object.Instantiate
            (
                _config.Heroes.First(x => x.Type == type).HeroPrefab, 
                spawnPoint
            );  
        }
    }
}
