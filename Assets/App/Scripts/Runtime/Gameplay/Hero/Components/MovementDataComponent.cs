using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct MovementDataComponent
    {
        public Vector3 Direction;
        public Quaternion Rotation;
        public float VerticalVelocity;
        public float Speed;
        public float MaxSpeed;
        public float TargetSpeed;
        public float RotateSpeed;
        public bool IsGround;
        public float MovementSmoothVelocity;
    }
}