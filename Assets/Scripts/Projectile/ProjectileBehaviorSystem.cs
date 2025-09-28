using DI;
using Projectile;
using System;
using UnityEngine;

namespace GameSystem
{
    public class ProjectileBehaviorSystem : MonoBehaviour
    {
        private GameFlowSystem _gameFlowSystem;
        private ProjectilePool _projectilePool;
        private GameEventBus _eventBus;

        private Action<ProjectileBase>[] _movementStrategies;

        [Inject]
        public void Construct(GameFlowSystem gameFlowSystem, ProjectilePool projectilePool, GameEventBus eventBus)
        {
            _gameFlowSystem = gameFlowSystem;
            _projectilePool = projectilePool;
            _eventBus = eventBus;


            RegisterMovement(ProjectileType.Homing, new ProjectileHomingMovement());
            RegisterMovement(ProjectileType.Straight, new ProjectileStraightMovement());
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

        private void RegisterMovement<TProjectile>(ProjectileType type, ProjectileMovementBase<TProjectile> behavior) where TProjectile : ProjectileBase
        {
            if (_movementStrategies == null)
            {
                int projectileTypeSize = Enum.GetValues(typeof(ProjectileType)).Length;
                _movementStrategies = new Action<ProjectileBase>[projectileTypeSize];
            }

            _movementStrategies[(int)type] = p => behavior.Move((TProjectile)p);
        }

        private void MoveProjectile(ProjectileBase projectile)
        {
            var action = _movementStrategies[(int)projectile.Type] ?? throw new InvalidOperationException(
                    $"Movement strategy not found for projectile type {projectile.Type}");
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

        private void OnUpdateTick()
        {
            foreach (var projectile in _projectilePool.ItemsInUse)
            {
                MoveProjectile(projectile);
                CheckLifeTime(projectile);
            }
        }
    }
}