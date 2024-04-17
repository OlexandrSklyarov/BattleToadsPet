using System;
using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/CharacterGravityConfig", fileName = "CharacterGravityConfig")]
    public sealed class CharacterGravityConfig : ScriptableObject
    {
        [field: SerializeField, Min(1f)] public float FallGravityMultiplier {get; private set;} = 10f;
        [field: SerializeField, Min(1f)] public float MinVerticalVelocity {get; private set;} = 2f;       
    }
}
