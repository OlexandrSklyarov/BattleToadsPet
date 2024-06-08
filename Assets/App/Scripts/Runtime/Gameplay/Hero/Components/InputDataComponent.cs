using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct InputDataComponent
    {
        public Vector2 MoveDirection;
        public bool IsJumpWasPressed;
        public bool IsJumpWasReleased;
        public bool IsJumpHold;
        public bool IsAttackWasPressed;
        public bool IsRunHold;
    }
}

