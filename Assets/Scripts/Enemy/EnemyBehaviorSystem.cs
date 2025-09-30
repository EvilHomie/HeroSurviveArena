using DI;
using Enemy;
using System;
using UnityEngine;

namespace GameSystem
{
    public class EnemyBehaviorSystem : MonoBehaviour
    {
        [SerializeField] float _moveTrashHold = 0.1f; // для отбраковки движения
        private GameFlowSystem _gameFlowSystem;
        private Player _player;
        private EnemiesPool _enemiesPool;

        private Action<EnemyBase>[] _movementStrategies;
        private Action<EnemyBase>[] _attackStrategies;

        [Inject]
        public void Construct(GameFlowSystem gameFlowSystem, Player player, EnemiesPool enemiesPool, GameEventBus eventBus)
        {
            _gameFlowSystem = gameFlowSystem;
            _player = player;
            _enemiesPool = enemiesPool;

            RegisterMovementStrategy(EnemyType.Shooter, new ShooterMovement(_moveTrashHold));
            RegisterMovementStrategy(EnemyType.Kamikadze, new KamikadzeMovement());

            RegisterAttackStrategy(EnemyType.Shooter, new ShooterAttack(eventBus));
            RegisterAttackStrategy(EnemyType.Kamikadze, new KamikadzeAttack(eventBus));
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
            _gameFlowSystem.SystemsUpdateTick += OnUpdateTick;
        }

        private void Unsubscribe()
        {
            _gameFlowSystem.SystemsUpdateTick -= OnUpdateTick;
        }

        private void RegisterMovementStrategy<TEnemy>(EnemyType type, EnemyMovementBase<TEnemy> behavior) where TEnemy : EnemyBase
        {
            if (_movementStrategies == null)
            {
                int enemyTypeSize = Enum.GetValues(typeof(EnemyType)).Length;
                _movementStrategies = new Action<EnemyBase>[enemyTypeSize];
            }

            _movementStrategies[(int)type] = enemy => behavior.MoveAndRotate((TEnemy)enemy, _player);
        }

        private void RegisterAttackStrategy<TEnemy>(EnemyType type, EnemyAttackBase<TEnemy> behavior) where TEnemy : EnemyBase
        {
            if (_attackStrategies == null)
            {
                int enemyTypeSize = Enum.GetValues(typeof(EnemyType)).Length;
                _attackStrategies = new Action<EnemyBase>[enemyTypeSize];
            }

            _attackStrategies[(int)type] = enemy => behavior.Attack((TEnemy)enemy, _player);
        }

        private void MoveEnemy(EnemyBase enemy)
        {
            var action = _movementStrategies[(int)enemy.Type] ?? throw new InvalidOperationException(
                    $"Movement strategy not found for enemy type {enemy.Type}");
            action.Invoke(enemy);
        }

        private void AttackPlayer(EnemyBase enemy)
        {
            var action = _attackStrategies[(int)enemy.Type] ?? throw new InvalidOperationException(
                    $"Attack strategy not found for enemy type {enemy.Type}");
            action.Invoke(enemy);
        }

        private void OnUpdateTick()
        {
            foreach (var enemy in _enemiesPool.ActiveItems)
            {
                MoveEnemy(enemy);
                AttackPlayer(enemy);
            }

            _enemiesPool.ReleasePendingItems();
        }
    }
}

