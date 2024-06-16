using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/EngineConfig", fileName = "EngineConfig")]
    public sealed class EngineConfig : ScriptableObject
    {
        [field: Header("[MOVING]"), SerializeField, Min(1f)] public float MaxWalkSpeed { get; private set; } = 12f;
        [field: SerializeField, Min(1f)] public float GroundAcceleration { get; private set; } = 5f;
        [field: SerializeField, Min(1f)] public float GroundDeceleration { get; private set; } = 20f;
        [field: SerializeField, Min(1f)] public float AirAcceleration { get; private set; } = 5f;
        [field: SerializeField, Min(1f)] public float AirDeceleration { get; private set; } = 5f;
        [field: SerializeField, Min(1f)] public float MaxRunSpeed { get; private set; } = 20f;

        [field: Space(20), Header("[ROTATE]"), SerializeField, Min(1f)] public float RotateSpeed { get; private set; } = 800f;

        [field: Space(20), Header("[JUMP]"), SerializeField, Min(1f)] public float JumpHeight { get; private set; } = 5f;
        [field: SerializeField, Min(1f)] public float MaxJumpHeight { get; private set; } = 5f;
        [field: SerializeField, Min(0.01f)] public float JumpTime { get; private set; } = 0.5f;
        [field: SerializeField, Min(1f)] public float InitialJumpVelocity { get; private set; } = 5f;
        [field: SerializeField, Range(1f, 1.1f)] public float JumpHeightCompensationFactor { get; private set; } = 1.054f;
        [field: SerializeField, Min(0.1f)] public float TimeTillJumpApex { get; private set; } = 0.35f;
        [field: SerializeField, Min(0.1f)] public float GravityOnReleaseMultiplier { get; private set; } = 2f;
        [field: SerializeField, Min(0.1f)] public float MaxUpSpeed { get; private set; } = 50f;
        [field: SerializeField, Min(0.1f)] public float MaxFallSpeed { get; private set; } = 26f;
        [field: SerializeField, Range(1, 5)] public int NumberofJumpsAllowed { get; private set; } = 2;

        [field: Space(20), Header("[JUMP CUT]"), SerializeField, Range(0.02f, 0.3f)] public float TimeForUpwardsCancel { get; private set; } = 0.027f;
        
        [field: Space(20), Header("[JUMP APEX]"), SerializeField, Range(0.5f, 1f)] public float ApexThreshold { get; private set; } = 0.97f;
        [field: SerializeField, Range(0.01f, 1f)] public float ApexHandTime { get; private set; } = 0.075f;

        [field: Space(20), Header("[JUMP BUFFER]"), SerializeField, Range(0f, 1f)] public float JumpBufferTime { get; private set; } = 0.125f;
        
        [field: Space(20), Header("[JUMP COYOTE TIME]"), SerializeField, Range(0f, 1f)] public float JumpCoyoteTime { get; private set; } = 0.1f;
    }
}