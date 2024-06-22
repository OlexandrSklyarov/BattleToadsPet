using UnityEngine;

namespace BT.Runtime.Gameplay.General.Components
{
    public struct AnimatorComponent
    {
        public Animator AnimatorRef;
        public bool IsPlayLocomotion;
        public bool JumpTriggered;
        public bool Landed;
        public bool Attacked;
        public int CurrentState;
        public float LockedTill;
    }
}
