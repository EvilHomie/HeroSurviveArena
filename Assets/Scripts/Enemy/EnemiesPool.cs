using Enemy;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class EnemiesPool : AbstractPool<EnemyBase>
    {
        private GameObject _enemiesContainer;
        private readonly List<string> _enemyNames = new();

        private void Awake()
        {
            _enemiesContainer = new GameObject("Pool_Container_Enemies");

            foreach (EnemyBase enemy in _config.Enemies)
            {
                if (enemy == null) continue;

                _enemyNames.Add(enemy.UsedName);
                CreateItemPool(enemy, _config.EnemyPoolStartCapacity, _config.EnemyPoolMaxCapacity, _enemiesContainer.transform, _config.EnemyPoolPrewarmCount);
            }
        }

        protected override void Subscribe()
        {
            _gameEventBus.EnemyDie += OnItemDeactivated;
            _gameEventBus.ChangeGameState += OnChangeGameState;
            _gameFlowSystem.UpdateTick += ReleaseInactive;
        }

        protected override void Unsubscribe()
        {
            _gameEventBus.EnemyDie -= OnItemDeactivated;
            _gameEventBus.ChangeGameState -= OnChangeGameState;
            _gameFlowSystem.UpdateTick -= ReleaseInactive;
        }

        public EnemyBase GetRandomEnemy()
        {
            var randomIndex = Random.Range(0, _enemyNames.Count);
            var randomName = _enemyNames[randomIndex];
            return Getitem(randomName);
        }

        private void OnChangeGameState(GameState gameState)
        {
            if (gameState == GameState.GameOver || gameState == GameState.Victory)
            {
                ReleaseAll();
            }
        }
    }
}