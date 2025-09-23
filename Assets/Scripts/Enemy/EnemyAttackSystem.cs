using DI;
using System;
using UnityEngine;

namespace GameSystem
{
    public class EnemyAttackSystem : MonoBehaviour
    {
        private GameFlowSystem _gameFlowSystem;
        private EnemiesPool _enemiesPool;

        [Inject]
        public void Construct(GameFlowSystem gameFlowSystem, EnemiesPool enemiesPool)
        {
            _gameFlowSystem = gameFlowSystem;
            _enemiesPool = enemiesPool;
            _gameFlowSystem.UpdateTick += OnUpdateTick;
        }

        private void OnUpdateTick()
        {
            foreach (var enemy in _enemiesPool.ActiveEnemies)
            {
                if (enemy.InAtackRange)
                {
                    enemy.Atack();
                }
            }
        }
    }
}

