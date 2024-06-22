using System;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/AnimationConfig", fileName = "AnimationConfig")]
    public sealed class AnimationConfig : ScriptableObject
    {        
        [field: SerializeField, Min(1f)] public float MaxFallVelocity {get; private set;} = 8f;   
        [field: SerializeField, Min(0.001f)] public float CrosfadeAnimime {get; private set;} = 0.1f;  
        [field: Space, SerializeField] public StateAnimation State {get; private set;}

        [Serializable]
        public class StateAnimation
        {
            [field: SerializeField, Min(0.001f)] public float AttackTime {get; private set;} = 1f;  
            [field: SerializeField, Min(0.001f)] public float LandingTime {get; private set;} = 1f; 
        } 
    }
}
