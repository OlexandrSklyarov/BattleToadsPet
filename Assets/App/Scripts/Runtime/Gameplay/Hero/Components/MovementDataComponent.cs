using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct MovementDataComponent
    {
        public Quaternion Rotation;
        public Vector3 Velocity;
        public float InitialJumpVelocity;
        public float Gravity;
        public bool IsGround;
        public bool IsJumping;
        public bool IsFalling;
    }
}