using DI;
using Enemy;
using Projectile;
using System;
using UnityEngine;

namespace GameSystem
{
    public class ProjectileBehaviorSystem : MonoBehaviour
    {
        [SerializeField] LayerMask _playerLayerMask;
        [SerializeField] LayerMask _enemyLayerMask;
        private GameFlowSystem _gameFlowSystem;
        private ProjectilePool _projectilePool;
        private GameEventBus _eventBus;

        private Action<ProjectileBase>[] _movementStrategies;
        private Action<ProjectileBase>[] _attackStrategies;

        [Inject]
        public void Construct(GameFlowSystem gameFlowSystem, ProjectilePool projectilePool, GameEventBus eventBus)
        {
            _gameFlowSystem = gameFlowSystem;
            _projectilePool = projectilePool;
            _eventBus = eventBus;


            RegisterMovement(ProjectileType.Homing, new ProjectileHomingMovement());
            RegisterMovement(ProjectileType.Straight, new ProjectileStraightMovement());

            RegisterAttack(OwnerType.Enemy, new ProjectileSimpleImpact<Player>(1, _playerLayerMask, OnProjectileDie, OnPlayerDie));
            RegisterAttack(OwnerType.Player, new ProjectileSimpleImpact<EnemyBase>(1, _enemyLayerMask, OnProjectileDie, OnEnemyDie));
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

        private void RegisterMovement<TProjectile>(ProjectileType type, ProjectileMovementBase<TProjectile> behavior) where TProjectile : ProjectileBase
        {
            if (_movementStrategies == null)
            {
                int projectileTypeSize = Enum.GetValues(typeof(ProjectileType)).Length;
                _movementStrategies = new Action<ProjectileBase>[projectileTypeSize];
            }

            _movementStrategies[(int)type] = p => behavior.Move((TProjectile)p);
        }

        private void RegisterAttack<T>(OwnerType type, ProjectileImpactBase<T> behavior) where T : IDamageable
        {
            if (_attackStrategies == null)
            {
                int projectileOwnerTypeSize = Enum.GetValues(typeof(OwnerType)).Length;
                _attackStrategies = new Action<ProjectileBase>[projectileOwnerTypeSize];
            }

            _attackStrategies[(int)type] = p => behavior.HandleImpact(p);
        }

        private void MoveProjectile(ProjectileBase projectile)
        {
            var action = _movementStrategies[(int)projectile.Type] ?? throw new InvalidOperationException(
                    $"Movement strategy not found for projectile type {projectile.Type}");
            action.Invoke(projectile);
        }

        private void ProcessCollision(ProjectileBase projectile)
        {
            var action = _attackStrategies[(int)projectile.OwnerType] ?? throw new InvalidOperationException(
                    $"Attack strategy not found for projectile type {projectile.Type} ownerType {projectile.OwnerType}");
            action.Invoke(projectile);
        }

        private void CheckLifeTime(ProjectileBase projectile)
        {
            projectile.LeftLifeTime -= Time.deltaTime;

            if (projectile.LeftLifeTime <= 0)
            {
                _eventBus.ProjectileDie(projectile);
            }
        }
        private void OnProjectileDie(ProjectileBase projectile)
        {
            _eventBus.ProjectileDie?.Invoke(projectile);
        }
        private void OnPlayerDie(Player player)
        {
            _eventBus.PlayerDie?.Invoke(player);
        }
        private void OnEnemyDie(EnemyBase enemy)
        {
            _eventBus.EnemyDie?.Invoke(enemy);
        }

        private void OnUpdateTick()
        {  
            foreach (var projectile in _projectilePool.ItemsInUse)
            {
                CheckLifeTime(projectile);
            }

            _projectilePool.ReleaseInactive();

            foreach (var projectile in _projectilePool.ItemsInUse)
            {
                ProcessCollision(projectile);
            }

            _projectilePool.ReleaseInactive();

            foreach (var projectile in _projectilePool.ItemsInUse)
            {
                MoveProjectile(projectile);
            }
        }
    }
}