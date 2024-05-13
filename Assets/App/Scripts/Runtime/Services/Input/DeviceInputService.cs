using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BT.Runtime.Services.Input
{
    public class DeviceInputService : IInputService
    {
        public Vector2 Movement => (!_isActive) ? Vector2.zero : _control.Player.Move.ReadValue<Vector2>();
        public bool IsAttack => _control.Player.Attack.WasPressedThisFrame();
        public bool IsJump {get; private set;}
        public bool IsRun {get; private set;}

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

            _control.Player.Run.started += OnRunHandler;
            _control.Player.Run.performed += OnRunHandler;
            _control.Player.Run.canceled += OnRunHandler;

            _control.Player.Jump.started += OnJumpHandler;
            _control.Player.Jump.canceled += OnJumpHandler;
        }        

        public void Disable()
        {
            if (!_isActive) return;

            _control.Disable();
            _isActive = false;

            _control.Player.Run.started -= OnRunHandler;
            _control.Player.Run.performed -= OnRunHandler;
            _control.Player.Run.canceled -= OnRunHandler;

            _control.Player.Jump.started -= OnJumpHandler;
            _control.Player.Jump.canceled -= OnJumpHandler;
        }

        private void OnJumpHandler(InputAction.CallbackContext context)
        {
            IsJump = context.ReadValue<float>() > 0;
        }

        private void OnRunHandler(InputAction.CallbackContext context)
        {
            IsRun = context.ReadValue<float>() > 0;
        }
    }
}