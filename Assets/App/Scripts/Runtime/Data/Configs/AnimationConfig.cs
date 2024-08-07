using System;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/AnimationConfig", fileName = "AnimationConfig")]
    public sealed class AnimationConfig : ScriptableObject
    {          
        [field: SerializeField, Min(0.001f)] public float CrosfadeAnimime {get; private set;} = 0.01f;  
        [field: SerializeField, Min(0.001f)] public float LandingTime {get; private set;} = 1f; 
        [field: SerializeField, Min(0.001f)] public float FallTimeThreshold {get; private set;} = 0.6f;          
    }
}
