using DI;
using System;
using UnityEngine;

namespace GameSystem
{
    public class PlayerControllSystem : MonoBehaviour
    {
        private IPlayerInput _input;
        private Player _player;

        [Inject]
        public void Constructor(IPlayerInput playerInput, Player player)
        {
            _input = playerInput;
            _player = player;
        }
        private void OnEnable()
        {
            _input.Subscrube();
            Subscribe();
        }

        private void OnDisable()
        {
            _input.Unsubscribe();
            Unsubscribe();
        }

        private void Subscribe()
        {
            _input.MoveAction += OnMove;
            _input.RotateAction += OnRotate;
            _input.StartAtackAction += OnAttack;
            _input.EndAtackAction += OnEndAttack;
        }
        private void Unsubscribe()
        {
            _input.MoveAction -= OnMove;
            _input.RotateAction -= OnRotate;
            _input.StartAtackAction -= OnAttack;
            _input.EndAtackAction -= OnEndAttack;
        }

        private void OnRotate(Vector3 direction)
        {
            _player.CachedTransform.rotation = Quaternion.LookRotation(direction);
        }

        private void OnMove(Vector3 direction)
        {
            _player.CachedTransform.Translate(_player.MoveSpeed * Time.deltaTime * direction, Space.World);
            _player.CachedPosition = _player.CachedTransform.position;
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