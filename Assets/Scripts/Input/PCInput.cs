using DI;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private Plane _groudPlane;
    private bool _isMoving;

    [Inject]
    public void Constructor(GameFlowSystem gameFlowSystem, PlayerContainer playerContainer, Camera camera)
    {
        _inputActions = new InputSystem_Actions();
        _gameFlowSystem = gameFlowSystem;
        _playerContainer = playerContainer;
        _camera = camera;
        _groudPlane = new(Vector3.up, Vector3.zero);
        _gameFlowSystem.CustomStart += Subscribes;
        _gameFlowSystem.OnAppQuit += OnQuit;
    }

    private void Subscribes()
    {
        _gameFlowSystem.CustomUpdate += OnUpdate;
        _gameFlowSystem.ChangeGameState += OnGameChangeState;
        _inputActions.Player.Move.performed += OnStartMoving;
        _inputActions.Player.Move.canceled += OnStopMoving;
        _inputActions.Player.Attack.performed += OnAttack;
        _inputActions.Player.Attack.canceled += OnEndAttack;
    }

    private void OnQuit()
    {
        _inputActions.Dispose();
        _gameFlowSystem.CustomUpdate -= OnUpdate;
        _gameFlowSystem.ChangeGameState -= OnGameChangeState;
        _inputActions.Player.Move.performed -= OnStartMoving;
        _inputActions.Player.Move.canceled -= OnStopMoving;
        _inputActions.Player.Attack.performed -= OnAttack;
        _inputActions.Player.Attack.canceled -= OnEndAttack;
    }

    private void OnUpdate()
    {
        RotateToMouse();

        if (_isMoving)
        {
            var inputDir = _inputActions.Player.Move.ReadValue<Vector2>();
            var moveDir = new Vector3(inputDir.x, 0, inputDir.y);
            MoveAction?.Invoke(moveDir);
        }
    }

    private void OnGameChangeState(GameFlowState gameFlow)
    {
        if (gameFlow == GameFlowState.Playing)
        {
            _inputActions.Player.Enable();
        }
        else
        {
            _inputActions.Player.Disable();
        }
    }

    private void RotateToMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (_groudPlane.Raycast(ray, out float distance))
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

    private void OnStartMoving(InputAction.CallbackContext context)
    {
        _isMoving = true;
    }

    private void OnStopMoving(InputAction.CallbackContext context)
    {
        _isMoving = false;
    }
}
