using DI;
using GameSystem;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CoreGamePlay
{
    public class EnemySpawnSystem : MonoBehaviour
    {
        private GameObject _enemiesContainer;
        private GameFlowSystem _gameFlowSystem;
        private Config _config;
        private EnemiesPool _enemyPool;
        private PlayerContainer _playerContainer;
        private float _enemySpawnRepeatRate;
        private float _enemySpawnTimer;
        private float _spawnRadius;
        private List<EnemyType> _enemyTypes;

        [Inject]
        public void Constructor(GameFlowSystem gameFlowSystem, Config config, EnemiesPool enemiesPool, PlayerContainer playerContainer)
        {
            _gameFlowSystem = gameFlowSystem;
            _config = config;
            _enemySpawnRepeatRate = config.EnemySpawnRepeatRate;
            _enemyPool = enemiesPool;
            _playerContainer = playerContainer;
            _spawnRadius = config.EnemySpawnRadius;
            _gameFlowSystem.ChangeGameState += OnChangeGameState;
            _gameFlowSystem.UpdateTick += OnUpdateTick;
        }

        private void Awake()
        {
            _enemiesContainer = new GameObject("Enemies_Container");
            _enemyTypes = new();

            foreach (Enemy enemy in _config.Enemies)
            {
                if (enemy == null) continue;

                _enemyTypes.Add(enemy.EnemyType);
                _enemyPool.AddEnemy(enemy, _config.EnemyPoolStartCapacity, _config.EnemyPoolMaxCapacity, _enemiesContainer.transform, _config.EnemyPoolPrewarmCount);
            }
        }

        private void OnChangeGameState(GameFlowState state)
        {
            if (state == GameFlowState.StartGame)
            {
                OnStartGame();
            }
            else if (state == GameFlowState.Victory || state == GameFlowState.GameOver)
            {
                OnGameEnd();
            }
        }

        private void OnStartGame()
        {
            _enemySpawnTimer = _enemySpawnRepeatRate;
        }

        private void OnGameEnd()
        {
            _enemyPool.ReturnAllActiveEnemies();
        }

        private void OnUpdateTick()
        {
            _enemySpawnTimer -= Time.deltaTime;

            if (_enemySpawnTimer <= 0)
            {
                _enemySpawnTimer = _enemySpawnRepeatRate;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            var enemy = GetRandomEnemy();
            var spawnPos = GetSpawnPos();
            var dirToPlayer = _playerContainer.transform.position - spawnPos;
            enemy.transform.position = spawnPos;
            enemy.transform.forward = dirToPlayer;
            enemy.gameObject.SetActive(true);
        }

        private Enemy GetRandomEnemy()
        {
            var randomIndex = Random.Range(0, _enemyTypes.Count);
            var randomType = _enemyTypes[randomIndex];
            return _enemyPool.GetEnemy(randomType);
        }

        private Vector3 GetSpawnPos()
        {
            float spawnAngle = Random.Range(0f, 360f);
            Vector3 spawnDir = Quaternion.Euler(0, spawnAngle, 0) * Vector3.forward;
            return _playerContainer.transform.position + spawnDir * _spawnRadius;
        }
    }
}


