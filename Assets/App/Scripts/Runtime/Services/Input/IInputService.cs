using UnityEngine;

namespace BT.Runtime.Services.Input
{
    public interface IInputService
    {
        public Vector2 Movement {get;}
        public bool IsAttackWasPressed {get;}
        public bool IsJumpWasPressed {get;}
        public bool IsJumpHold {get;}
        public bool IsJumpWasReleased {get;}
        public bool IsRunHold {get;}

        void Enable();
        void Disable();
    }
}