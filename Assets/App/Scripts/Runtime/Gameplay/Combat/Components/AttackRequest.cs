using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Combat.Components
{
    public struct AttackRequest
    {
        public Vector3 Position;
        public Vector3 AttackDirection;
        public float Radius;
        public float Damage;
    }
}
