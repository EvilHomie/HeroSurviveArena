using DI;
using UnityEngine;

namespace GameSystem
{
    public class CameraSystem : MonoBehaviour
    {
        private Vector3 _deffOffset;
        private Camera _camera;
        private Player _player;
        private GameFlowSystem _gameFlowSystem;

        [Inject]
        public void Constructor(Player player, GameFlowSystem gameFlow, Camera camera)
        {
            _player = player;
            _camera = camera;
        }

        private void Awake()
        {
            _deffOffset = _camera.transform.position;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
        private void Subscribe()
        {
            _gameFlowSystem.LateUpdateTick += OnLateUpdateTick;
        }
        private void Unsubscribe()
        {
            _gameFlowSystem.LateUpdateTick -= OnLateUpdateTick;
        }

        private void OnLateUpdateTick()
        {
            _camera.transform.position = _player.CachedPosition + _deffOffset;
        }
    }
}

