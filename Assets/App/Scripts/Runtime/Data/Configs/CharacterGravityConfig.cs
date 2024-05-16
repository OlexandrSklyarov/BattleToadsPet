using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/CharacterGravityConfig", fileName = "CharacterGravityConfig")]
    public sealed class CharacterGravityConfig : ScriptableObject
    {
        [field: SerializeField, Min(0.01f)] public float MinVerticalVelocity {get; private set;} = 0.05f;
        [field: SerializeField, Min(0.01f)] public float FallMultiplier {get; private set;} = 2f;
        [field: SerializeField, Min(0.01f)] public float MaxFallVelocity {get; private set;} = 20f;
    }
}
