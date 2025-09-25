using DI;
using Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class EnemyBehaviorSystem : MonoBehaviour
    {
        [SerializeField] float _moveTrashHold = 0.1f; // для отбраковки движения
        private GameFlowSystem _gameFlowSystem;
        private Player _player;
        private EnemiesPool _enemiesPool;
        private float _sqrMoveTrashHold;

        private readonly Dictionary<Type, IMovementBehaviorBase> _movementStrategies = new();
        private readonly Dictionary<Type, IAttackBehaviorBase> _attackStrategies = new();

        [Inject]
        public void Construct(GameFlowSystem gameFlowSystem, Player player, EnemiesPool enemiesPool, GameEventBus eventBus)
        {
            _gameFlowSystem = gameFlowSystem;
            _player = player;
            _enemiesPool = enemiesPool;
            _sqrMoveTrashHold = _moveTrashHold * _moveTrashHold;

            _movementStrategies[typeof(Kamikadze)] = new KamikadzeMoveBehavior();
            _movementStrategies[typeof(EnemyShooter)] = new RangedMoveBehavior();
            _attackStrategies[typeof(Kamikadze)] = new KamikadzeAttackBehavior(eventBus);
            _attackStrategies[typeof(EnemyShooter)] = new RangedAttackBehavior(eventBus);
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
            _gameFlowSystem.UpdateTick += OnUpdateTick;
        }

        private void Unsubscribe()
        {
            _gameFlowSystem.UpdateTick -= OnUpdateTick;
        }

        private void OnUpdateTick()
        {
            foreach (var enemy in _enemiesPool.ItemsInUse)
            {
                if (_movementStrategies.TryGetValue(enemy.CashedType, out var moveBehavior))
                {
                    moveBehavior.MoveAndRotate(enemy, _player, _sqrMoveTrashHold);
                }
                else
                {
                    throw new Exception($"Нет стратегии движения для {enemy.CashedType}");
                }

                if (_attackStrategies.TryGetValue(enemy.CashedType, out var attackBehavior))
                {
                    attackBehavior.CheckDistance(enemy, _player);
                    attackBehavior.Attack(enemy, _player);
                }
                else
                {
                    throw new Exception($"Нет стратегии атаки для {enemy.CashedType}");
                }
            }
        }
    }
}

