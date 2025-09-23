using DI;
using GameSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class PCInput : IPlayerInput
    {
        public event Action<Vector3> MoveAction;
        public event Action<Vector3> RotateAction;
        public event Action StartAtackAction;
        public event Action EndAtackAction;

        private InputSystem_Actions _inputActions;
        private GameFlowSystem _gameFlowSystem;
        private PlayerContainer _playerContainer;
        private Camera _camera;
        private Plane _raycastPlane;
        private bool _isActive;

        [Inject]
        public void Constructor(GameFlowSystem gameFlowSystem, PlayerContainer playerContainer, Camera camera)
        {
            _inputActions = new InputSystem_Actions();
            _gameFlowSystem = gameFlowSystem;
            _playerContainer = playerContainer;
            _camera = camera;
            _raycastPlane = new(Vector3.up, Vector3.zero);
            Subscribes();
        }

        private void Subscribes()
        {
            _gameFlowSystem.UpdateTick += OnUpdateTick;
            _gameFlowSystem.ChangeGameState += OnGameChangeState;
            _inputActions.Player.Attack.performed += OnAttack;
            _inputActions.Player.Attack.canceled += OnEndAttack;
        }

        private void OnUpdateTick()
        {
            if (!_isActive)
            {
                return;
            }

            RotateToMouse();
            var inputDir = _inputActions.Player.Move.ReadValue<Vector2>();

            if (inputDir != Vector2.zero)
            {
                var moveDir = new Vector3(inputDir.x, 0, inputDir.y);
                MoveAction?.Invoke(moveDir);
            }
        }

        private void OnGameChangeState(GameFlowState gameFlow)
        {
            _isActive = gameFlow == GameFlowState.Playing;

            if (_isActive) _inputActions.Player.Enable();
            else _inputActions.Player.Disable();
        }

        private void RotateToMouse()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (_raycastPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                Vector3 direction = hitPoint - _playerContainer.transform.position;
                direction.y = 0;
                RotateAction?.Invoke(direction);
            }
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            StartAtackAction?.Invoke();
        }

        private void OnEndAttack(InputAction.CallbackContext context)
        {
            EndAtackAction?.Invoke();
        }
    }
}
