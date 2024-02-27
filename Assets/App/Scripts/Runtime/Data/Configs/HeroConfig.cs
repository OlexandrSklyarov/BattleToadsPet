using System;
using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/HeroConfig", fileName = "HeroConfig")]
    public sealed class HeroConfig : ScriptableObject
    {
        [field: SerializeField] public HeroView HeroPrefab {get; private set;}
        [field: SerializeField] public EngineConfig Engine {get; private set;}

        [Serializable]
        public sealed class EngineConfig
        {
            [field: SerializeField, Min(1f)] public float MoveSpeed {get; private set;} = 3f;
        }
    }
}