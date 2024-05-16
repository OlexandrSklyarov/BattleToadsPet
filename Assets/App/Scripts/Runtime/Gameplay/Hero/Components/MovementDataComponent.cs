using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct MovementDataComponent
    {
        public Vector3 Direction;
        public Quaternion Rotation;
        public float VerticalVelocity;
        public float MovementSmoothVelocity;
        public float Speed;
        public float MaxSpeed;
        public float DesiredSpeed;
        public float RotateSpeed;
        public float InitialJumpVelocity;
        public float Gravity;
        public bool IsGround;
        public bool IsJumping;
        public bool IsFalling;
    }
}