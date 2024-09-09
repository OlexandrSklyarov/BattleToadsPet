using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/EngineConfig", fileName = "EngineConfig")]
    public sealed class EngineConfig : ScriptableObject
    {
        [field: Header("[MOVING]"), SerializeField, Min(1f)] public float MaxWalkSpeed { get; private set; } = 12f;
        [field: SerializeField, Min(0.01f)] public float GroundAcceleration { get; private set; } = 5f;
        [field: SerializeField, Min(0.01f)] public float GroundDeceleration { get; private set; } = 20f;
        [field: SerializeField, Min(0.01f)] public float AirAcceleration { get; private set; } = 5f;
        [field: SerializeField, Min(0.01f)] public float AirDeceleration { get; private set; } = 5f;
        [field: SerializeField, Min(1f)] public float MaxRunSpeed { get; private set; } = 20f;

        [field: Space(20), Header("[ROTATE]"), SerializeField, Min(1f)] public float RotateSpeed { get; private set; } = 800f;

        [field: Space(20), Header("[JUMP]"), SerializeField, Min(1f)] public float MaxJumpHeight { get; private set; } = 5f;
        [field: SerializeField, Min(0.01f)] public float JumpTime { get; private set; } = 0.5f;
        [field: SerializeField, Min(0.01f)] public float FallMultiplier { get; private set; } = 2f;        
        [field: SerializeField, Min(0.1f)] public float MaxUpSpeed { get; private set; } = 50f;
        [field: SerializeField, Min(0.1f)] public float MaxFallSpeed { get; private set; } = 26f;
    }
}