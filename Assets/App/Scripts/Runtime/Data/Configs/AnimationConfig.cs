using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/AnimationConfig", fileName = "AnimationConfig")]
    public sealed class AnimationConfig : ScriptableObject
    {        
        [field: SerializeField, Min(1f)] public float MaxFallVelocity {get; private set;} = 8f;   
        [field: SerializeField, Min(0.001f)] public float CrosfadeAnimime {get; private set;} = 0.1f;   
    }
}
