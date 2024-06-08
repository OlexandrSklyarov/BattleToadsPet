using UnityEngine;
using UnityEngine.InputSystem;

namespace BT.Runtime.Services.Input
{
    public class DeviceInputService : IInputService
    {
        public Vector2 Movement => (!_isActive) ? Vector2.zero : _control.Player.Move.ReadValue<Vector2>();
        public bool IsAttackWasPressed => _control.Player.Attack.WasPressedThisFrame();
        public bool IsJumpWasPressed => _control.Player.Jump.WasPressedThisFrame();
        public bool IsJumpHold => _control.Player.Jump.IsPressed();
        public bool IsJumpWasReleased => _control.Player.Jump.WasPressedThisFrame();
        public bool IsRunHold =>_control.Player.Run.IsPressed();

        private readonly PlayerControl _control;
        private bool _isActive;


        public DeviceInputService()
        {
            _control = new PlayerControl();

            Enable();
        }

        public void Enable()
        {
            if (_isActive) return;

            _control.Enable();
            _isActive = true;
        }        

        public void Disable()
        {
            if (!_isActive) return;

            _control.Disable();
            _isActive = false;
        }
    }
}