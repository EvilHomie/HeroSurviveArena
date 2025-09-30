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

            foreach (EnemyBase enemy in Config.Enemies)
            {
                if (enemy == null) continue;

                _enemyNames.Add(enemy.UsedName);
                CreateItemPool(enemy, Config.EnemyPoolStartCapacity, Config.EnemyPoolMaxCapacity, _enemiesContainer.transform, Config.EnemyPoolPrewarmCount);
            }
        }

        protected override void Subscribe()
        {
            GameEventBus.EnemyDie += OnItemDeactivated;
            GameEventBus.ChangeGameState += OnChangeGameState;
        }

        protected override void Unsubscribe()
        {
            GameEventBus.EnemyDie -= OnItemDeactivated;
            GameEventBus.ChangeGameState -= OnChangeGameState;
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