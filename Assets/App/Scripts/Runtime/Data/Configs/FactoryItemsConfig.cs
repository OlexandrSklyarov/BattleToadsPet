using System;
using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/FactoryItemsConfig", fileName = "FactoryItemsConfig")]
    public sealed class FactoryItemsConfig : ScriptableObject
    {
        [field: SerializeField] public HeroItem[] Heroes {get; private set;}

        [Serializable]
        public sealed class HeroItem
        {
            [field: SerializeField] public HeroType Type {get; private set;}
            [field: SerializeField] public HeroView HeroPrefab {get; private set;}
        }        
    }
}

