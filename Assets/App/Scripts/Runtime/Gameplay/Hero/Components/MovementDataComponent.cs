using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct MovementDataComponent
    {
        public Quaternion Rotation;
        public Vector3 Velocity;
        public float InitialJumpVelocity;
        public float AdjustedJumpHeight;
        public float Gravity;
        public float VerticalVelocity;
        public float FastFallTime;
        public float FastFallReleasedSpeed;
        public int NumberOfJumpsUsed;
        public float ApexPoint;
        public float TimePastApexThreshold;
        public float JumpBufferTimer;
        public float CoyoteTime;
        public bool IsJumpReleasedDuringBuffer;
        public bool IsPastApexThreshold;
        public bool IsGround;
        public bool IsJumping;
        public bool IsFastFalling;
        public bool IsFalling;
        public bool IsBumpedHead;
        public bool IsLanded;
    }
}