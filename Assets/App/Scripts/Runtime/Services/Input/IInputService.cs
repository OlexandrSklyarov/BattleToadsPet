using UnityEngine;

namespace BT.Runtime.Services.Input
{
    public interface IInputService
    {
        public Vector2 Movement {get;}
        public bool IsAttack {get;}
        public bool IsJump {get;}
        public bool IsRun {get;}

        void Enable();
        void Disable();
    }
}