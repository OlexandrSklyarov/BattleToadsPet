using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/CharacterGravityConfig", fileName = "CharacterGravityConfig")]
    public sealed class CharacterGravityConfig : ScriptableObject
    {
        [field: SerializeField] public LayerMask GroundLayer {get; private set;}
        [field: SerializeField, Min(0.01f)] public float GroundDetectionRayLength {get; private set;} = 0.02f;
        [field: SerializeField, Min(0.01f)] public float HeadDetectionRayLength {get; private set;} = 0.02f;
        [field: SerializeField, Min(0.01f)] public float HeadWidth {get; private set;} = 0.75f;        
        [field: SerializeField, Range(-100f, 100f)] public float GroundGravity {get; private set;} = -0.5f;
    }
}
