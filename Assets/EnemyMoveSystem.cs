using DI;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class EnemyMoveSystem : MonoBehaviour
    {
        private GameEventBus _eventBus;
        private GameFlowSystem _gameFlowSystem;
        private HashSet<Enemy> _rangedEnemies;
        private HashSet<Enemy> _kamikadzeEnemies;
        private PlayerContainer _playerContainer;
        private readonly float _moveTrashHold = 1f; // для отбраковки движения

        [Inject]
        public void Constructor(GameEventBus eventBus, GameFlowSystem gameFlowSystem, PlayerContainer playerContainer)
        {
            _eventBus = eventBus;
            _gameFlowSystem = gameFlowSystem;
            _playerContainer = playerContainer;
            _gameFlowSystem.ChangeGameState += OnChangeGameState;
            _gameFlowSystem.UpdateTick += OnUpdateTick;
            _eventBus.EnemySpawn += OnEnemySpawn;
            _eventBus.EnemyDie += OnEnemyDie;
        }

        private void OnEnemySpawn(Enemy enemy)
        {
            if (enemy.EnemyType == EnemyType.KamikadzeSlow || enemy.EnemyType == EnemyType.KamikadzeFast)
            {
                _kamikadzeEnemies.Add(enemy);
            }
            else if (enemy.EnemyType == EnemyType.Ranged)
            {
                _rangedEnemies.Add(enemy);
            }
        }

        private void OnEnemyDie(Enemy enemy)
        {
            if (enemy.EnemyType == EnemyType.KamikadzeSlow || enemy.EnemyType == EnemyType.KamikadzeFast)
            {
                _kamikadzeEnemies.Remove(enemy);
            }
            else if (enemy.EnemyType == EnemyType.Ranged)
            {
                _rangedEnemies.Remove(enemy);
            }
        }

        private void Awake()
        {
            _rangedEnemies = new();
            _kamikadzeEnemies = new();
        }

        private void OnChangeGameState(GameFlowState state)
        {
            if (state == GameFlowState.Victory || state == GameFlowState.GameOver)
            {
                OnGameEnd();
            }
        }

        private void OnGameEnd()
        {
            _rangedEnemies.Clear();
            _kamikadzeEnemies.Clear();
        }

        private void OnUpdateTick()
        {
            var playerPos = _playerContainer.transform.position;

            foreach (var enemy in _kamikadzeEnemies)
            {
                Vector3 toPlayer = playerPos - enemy.transform.position;
                MoveToPlayer(enemy, playerPos, toPlayer);
                RotateToPlayer(enemy, toPlayer);
            }

            foreach (var enemy in _rangedEnemies)
            {
                Vector3 toPlayer = playerPos - enemy.transform.position;
                StayOnDistance(enemy, playerPos, toPlayer);
                RotateToPlayer(enemy, toPlayer);
            }
        }

        private void MoveToPlayer(Enemy enemy, Vector3 playerPos, Vector3 toPlayer)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, playerPos, enemy.MoveSpeed * Time.deltaTime);
        }

        private void StayOnDistance(Enemy enemy, Vector3 playerPos, Vector3 toPlayer)
        {
            float desiredDistance = enemy.DesiredDistance;
            float sqrDistance = toPlayer.sqrMagnitude;
            float sqrDesired = desiredDistance * desiredDistance;

            if (Mathf.Abs(sqrDistance - sqrDesired) > _moveTrashHold)
            {
                Vector3 targetPos = playerPos - toPlayer.normalized * desiredDistance;
                enemy.transform.position = Vector3.MoveTowards(
                    enemy.transform.position,
                    targetPos,
                    enemy.MoveSpeed * Time.deltaTime
                );
            }
        }

        private void RotateToPlayer(Enemy enemy, Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                enemy.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}

