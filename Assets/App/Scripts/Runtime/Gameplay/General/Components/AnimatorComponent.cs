using UnityEngine;

namespace BT.Runtime.Gameplay.General.Components
{
    public struct AnimatorComponent
    {
        public Animator AnimatorRef;
        public int CurrentState;
        public float LockedTill;
        public bool Landed;
        public bool Attacked;
    }
}
