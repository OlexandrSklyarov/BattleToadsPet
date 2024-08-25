using System;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/AnimationConfig", fileName = "AnimationConfig")]
    public sealed class AnimationConfig : ScriptableObject
    {          
        [field: SerializeField, Range(0.001f, 1f)] public float DefaultCrosfadeAnimime {get; private set;} = 0.01f;  
        [field: SerializeField, Range(0.001f, 0.09f)] public float AttackCrosfadeAnimime {get; private set;} = 0.01f;  
        [field: SerializeField, Range(0.001f, 2f)] public float LandingTime {get; private set;} = 0.4f; 
        [field: SerializeField, Min(0.001f)] public float FallTimeThreshold {get; private set;} = 0.6f;          
    }
}
