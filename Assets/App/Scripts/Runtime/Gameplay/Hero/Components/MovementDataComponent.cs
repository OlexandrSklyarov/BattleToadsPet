using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct MovementDataComponent
    {
        public Quaternion Rotation;
        public Vector3 Velocity;
        public float InitialJumpVelocity;
        public float Gravity;
        public float VerticalVelocity;
        public bool IsGround;
        public bool IsJumping;
        public bool IsFalling;
        public bool IsBumpedHead;
        public bool IsLanded;
        public float FallTime ;
        public bool IsStartJump;
        public bool IsGroundFar;
    }
}