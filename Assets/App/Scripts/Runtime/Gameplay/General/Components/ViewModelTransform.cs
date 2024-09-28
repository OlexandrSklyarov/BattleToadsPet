using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Components
{
    public struct ViewModelTransform
    {
        public Transform ModelTransformRef;
        public Quaternion LookAt => Vector3Math.DirToQuaternion(LookAtDirection);
        public Vector3 LookAtDirection;
        public float RotateSpeed;
    }
}

