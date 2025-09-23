using DI;
using UnityEngine;

namespace GameSystem
{
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
            gameFlow.LateUpdateTick += OnLateUpdateTick;
        }

        private void Awake()
        {
            _deffOffset = _camera.transform.position;
        }

        private void OnLateUpdateTick()
        {
            _camera.transform.position = _playerContainer.transform.position + _deffOffset;
        }
    }
}

