using System;
using BT.Runtime.Gameplay.Enemy.View;
using BT.Runtime.Gameplay.Views.Hero;
using NaughtyAttributes;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/FactoryItemsConfig", fileName = "FactoryItemsConfig")]
    public sealed class FactoryItemsConfig : ScriptableObject
    {
        [field: SerializeField] public HeroItem[] Heroes {get; private set;}
        [field: HorizontalLine, Space(20), SerializeField] public EnemyItem[] Enemies {get; private set;}

        [Serializable]
        public sealed class HeroItem
        {
            [field: SerializeField] public HeroType Type {get; private set;}
            [field: SerializeField] public HeroView HeroPrefab {get; private set;}
        }      

        [Serializable]
        public sealed class EnemyItem
        {
            [field: SerializeField] public EnemyType Type {get; private set;}
            [field: SerializeField] public EnemyView Prefab {get; private set;}
        }   
    }
}

