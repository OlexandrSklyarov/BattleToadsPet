using UnityEngine;

namespace BT.Runtime.Gameplay.General.Components
{
    public struct AnimatorController
    {
        public Animator AnimatorRef;
        public int CurrentState;
        public float LockedTill;
        public bool Landed;
    }
}
