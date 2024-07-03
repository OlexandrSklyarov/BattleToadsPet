using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct MovementDataComponent
    {
        public Quaternion Rotation;        
        public float InitialJumpVelocity;
        public float Gravity;
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