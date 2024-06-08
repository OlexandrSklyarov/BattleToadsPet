using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/EngineConfig", fileName = "EngineConfig")]
    public sealed class EngineConfig : ScriptableObject
    {
        [field: SerializeField, Min(1f)] public float MaxWalkSpeed { get; private set; } = 12f;
        [field: SerializeField, Min(1f)] public float GroundAcceleration { get; private set; } = 5f;
        [field: SerializeField, Min(1f)] public float GroundDeceleration { get; private set; } = 20f;
        [field: SerializeField, Min(1f)] public float AirAcceleration { get; private set; } = 5f;
        [field: SerializeField, Min(1f)] public float AirDeceleration { get; private set; } = 5f;
        [field: SerializeField, Min(1f)] public float MaxRunSpeed { get; private set; } = 20f;

        [field: Space(20), SerializeField, Min(1f)] public float RotateSpeed { get; private set; } = 800f;

        [field: Space(20), SerializeField, Min(1f)] public float MaxJumpHeight { get; private set; } = 2f;
        [field: SerializeField, Min(0.1f)] public float JumpTime { get; private set; } = 0.5f;
        [field: Space, SerializeField] public bool IsChangeGravityPrmInRuntime { get; private set; } = true;
    }
}