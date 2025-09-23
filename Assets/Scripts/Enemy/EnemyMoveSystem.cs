using DI;
using Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class EnemyMoveSystem : MonoBehaviour
    {
        [SerializeField] float _moveTrashHold = 0.1f; // для отбраковки движения
        private GameFlowSystem _gameFlowSystem;
        private PlayerContainer _playerContainer;
        private EnemiesPool _enemiesPool;
        private float _sqrMoveTrashHold;

        private readonly Dictionary<Type, IMovementBehaviorBase> _movementStrategies = new();

        [Inject]
        public void Construct(GameFlowSystem gameFlowSystem, PlayerContainer playerContainer, EnemiesPool enemiesPool, Config config)
        {
            _gameFlowSystem = gameFlowSystem;
            _playerContainer = playerContainer;
            _enemiesPool = enemiesPool;
            _sqrMoveTrashHold = _moveTrashHold * _moveTrashHold;
            _gameFlowSystem.UpdateTick += OnUpdateTick;

            _movementStrategies[typeof(Kamikadze)] = new KamikadzeMoveBehavior();
            _movementStrategies[typeof(Ranged)] = new RangedMoveBehavior();
        }

        private void OnUpdateTick()
        {
            var playerPos = _playerContainer.transform.position;

            foreach (var enemy in _enemiesPool.ActiveEnemies)
            {
                if (_movementStrategies.TryGetValue(enemy.EnemyType, out var behavior))
                {
                    behavior.MoveAndRotate(enemy, playerPos, _sqrMoveTrashHold);
                }
                else
                {
                    throw new Exception($"Нет стратегии движения для {enemy.EnemyType}");
                }
            }
        }
    }
}

