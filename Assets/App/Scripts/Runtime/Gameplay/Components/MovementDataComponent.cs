using UnityEngine;

namespace BT.Runtime.Gameplay.Components
{
    public struct MovementDataComponent
    {
        public Vector3 Direction;
        public Quaternion Rotation;
        public float VerticalVelocity;
        public float Speed;
        public float RotateSpeed;
        public bool IsGround;
    }
}