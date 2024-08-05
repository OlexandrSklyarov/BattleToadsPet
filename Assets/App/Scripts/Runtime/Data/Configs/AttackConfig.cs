using System;
using BT.Runtime.Gameplay.Hero.Services.Attack;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/AttackConfig", fileName = "AttackConfig")]
    public sealed class AttackConfig : ScriptableObject
    {        
        [field: SerializeField, Min(0.001f)] public float Delay {get; private set;} = 0.5f;
        [field: SerializeField, Min(0.001f)] public float Radius {get; private set;} = 1f;
        [field: SerializeField, Min(0.001f)] public float PowerDamageMultiplier {get; private set;} = 3f;
        [field: Space(20), SerializeField] public DamagePoint[] Damages {get; private set;}

        [Serializable]
        public sealed class DamagePoint
        {
            [field: SerializeField] public AttackPointType Type {get; private set;}
            [field: SerializeField, Min(0.001f)] public float Power {get; private set;} = 1f;

        }
    }
}