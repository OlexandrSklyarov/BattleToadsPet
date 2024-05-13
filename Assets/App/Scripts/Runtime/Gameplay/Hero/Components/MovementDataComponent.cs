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
        public float TargetSpeed;
        public float RotateSpeed;
        public float JumpTime;
        public bool IsGround;
    }
}