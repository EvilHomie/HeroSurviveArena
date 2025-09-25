using DI;
using Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class ProjectileBehaviorSystem : MonoBehaviour
    {
        private GameFlowSystem _gameFlowSystem;
        private ProjectilePool _projectilePool;

        private readonly Dictionary<Type, IMovementBehaviorBase> _movementStrategies = new();
        //private readonly Dictionary<Type, IAttackBehaviorBase> _attackStrategies = new();

        [Inject]
        public void Construct(GameFlowSystem gameFlowSystem, ProjectilePool projectilePool)
        {
            _gameFlowSystem = gameFlowSystem;
            _projectilePool = projectilePool;

            _movementStrategies[typeof(ProjectileStraight)] = new ProjectileStraightBehavior();
            _movementStrategies[typeof(ProjectileHoming)] = new ProjectileHomingBehavior();


            //_attackStrategies[typeof(Kamikadze)] = new KamikadzeAttackBehavior(eventBus);
            //_attackStrategies[typeof(Ranged)] = new RangedAttackBehavior(eventBus);
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
            foreach (var projectile in _projectilePool.ItemsInUse)
            {
                if (_movementStrategies.TryGetValue(projectile.CashedType, out var moveBehavior))
                {
                    moveBehavior.Move(projectile);
                }
                else
                {
                    throw new Exception($"Нет стратегии движения для {projectile.CashedType}");
                }

                //if (_attackStrategies.TryGetValue(enemy.CashedType, out var attackBehavior))
                //{
                //    attackBehavior.CheckDistance(enemy, _player);
                //    attackBehavior.Attack(enemy, _player);
                //}
                //else
                //{
                //    throw new Exception($"Нет стратегии атаки для {enemy.CashedType}");
                //}
            }
        }
    }
}