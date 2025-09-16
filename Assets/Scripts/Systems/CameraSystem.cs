using DI;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    private Vector3 _deffOffset;
    private Camera _camera;
    private PlayerContainer _playerContainer;

    [Inject]
    public void Constructor(PlayerContainer playerContainer, GameFlowSystem gameFlow, Camera camera)
    {
        _playerContainer = playerContainer;
        _camera = camera;
        gameFlow.CustomStart += OnStart;
        gameFlow.CustomLateUpdate += OnLateUpdate;
    }

    private void OnStart()
    {
        _deffOffset = _camera.transform.position;
    }

    private void OnLateUpdate()
    {
        _camera.transform.position = _playerContainer.transform.position + _deffOffset;
    }
}
