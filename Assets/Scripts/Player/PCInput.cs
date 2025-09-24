using DI;
using GameSystem;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class PCInput : IPlayerInput
    {
        public Action<Vector3> MoveAction { get; set; }
        public Action<Vector3> RotateAction { get; set; }
        public Action StartAtackAction { get; set; }
        public Action EndAtackAction { get; set; }

        private InputSystem_Actions _inputActions;
        private GameFlowSystem _gameFlowSystem;
        private GameEventBus _eventBus;
        private Player _player;
        private Camera _camera;
        private Plane _raycastPlane;
        private bool _isActive;

        [Inject]
        public void Constructor(GameFlowSystem gameFlowSystem, Player playerContainer, Camera camera, GameEventBus eventBus)
        {
            _inputActions = new InputSystem_Actions();
            _gameFlowSystem = gameFlowSystem;
            _eventBus = eventBus;
            _player = playerContainer;
            _camera = camera;
            _raycastPlane = new(Vector3.up, Vector3.zero);
        }

        public void Subscrube()
        {
            _gameFlowSystem.UpdateTick += OnUpdateTick;
            _eventBus.ChangeGameState += OnGameChangeState;
            _inputActions.Player.Attack.performed += OnAttack;
            _inputActions.Player.Attack.canceled += OnEndAttack;
        }

        public void Unsubscribe()
        {
            _gameFlowSystem.UpdateTick -= OnUpdateTick;
            _eventBus.ChangeGameState -= OnGameChangeState;
            _inputActions.Player.Attack.performed -= OnAttack;
            _inputActions.Player.Attack.canceled -= OnEndAttack;
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

        private void OnGameChangeState(GameState gameFlow)
        {
            _isActive = gameFlow == GameState.Playing;

            if (_isActive) _inputActions.Player.Enable();
            else _inputActions.Player.Disable();
        }

        private void RotateToMouse()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (_raycastPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                Vector3 direction = hitPoint - _player.CachedPosition;
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
