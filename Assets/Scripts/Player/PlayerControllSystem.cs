using DI;
using UnityEngine;

namespace GameSystem
{
    public class PlayerControllSystem : MonoBehaviour
    {
        private IPlayerInput _input;
        private float _moveSpeed;
        private PlayerContainer _playerContainer;

        [Inject]
        public void Constructor(IPlayerInput playerInput, Config config, PlayerContainer playerContainer)
        {
            _input = playerInput;
            _moveSpeed = config.PlayerSpeed;
            _playerContainer = playerContainer;

            _input.MoveAction += OnMove;
            _input.RotateAction += OnRotate;
            _input.StartAtackAction += OnAttack;
            _input.EndAtackAction += OnEndAttack;
        }

        private void OnRotate(Vector3 direction)
        {
            _playerContainer.transform.rotation = Quaternion.LookRotation(direction);
        }

        private void OnMove(Vector3 direction)
        {
            _playerContainer.transform.Translate(_moveSpeed * Time.deltaTime * direction, Space.World);
        }
        private void OnAttack()
        {
            Debug.Log("START ATACK");
        }

        private void OnEndAttack()
        {
            Debug.Log("END ATACK");
        }
    }
}