using DI;
using Enemy;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystem
{
    public class EnemySpawnSystem : MonoBehaviour
    {
        private GameObject _enemiesContainer;
        private GameFlowSystem _gameFlowSystem;
        private Config _config;
        private EnemiesPool _enemyPool;
        private Player _player;
        private GameEventBus _eventBus;
        private float _enemySpawnRepeatRate;
        private float _enemySpawnTimer;
        private float _spawnRadius;
        private List<string> _enemyNames;

        [Inject]
        public void Constructor(GameFlowSystem gameFlowSystem, Config config, EnemiesPool enemiesPool, Player player, GameEventBus eventBus)
        {
            _gameFlowSystem = gameFlowSystem;
            _config = config;
            _enemySpawnRepeatRate = config.EnemySpawnRepeatRate;
            _enemyPool = enemiesPool;
            _player = player;
            _spawnRadius = config.EnemySpawnRadius;
            _eventBus = eventBus;            
        }

        private void Awake()
        {
            _enemiesContainer = new GameObject("Enemies_Container");
            _enemyNames = new();

            foreach (AbstractEnemy enemy in _config.Enemies)
            {
                if (enemy == null) continue;

                _enemyNames.Add(enemy.UsedName);
                _enemyPool.CreateItemPool(enemy, _config.EnemyPoolStartCapacity, _config.EnemyPoolMaxCapacity, _enemiesContainer.transform, _config.EnemyPoolPrewarmCount);
            }
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
            _eventBus.ChangeGameState += OnChangeGameState;
            _gameFlowSystem.UpdateTick += OnUpdateTick;
        }

        private void Unsubscribe()
        {
            _eventBus.ChangeGameState -= OnChangeGameState;
            _gameFlowSystem.UpdateTick -= OnUpdateTick;
        }

        private void OnChangeGameState(GameState state)
        {
            if (state == GameState.StartGame)
            {
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            _enemySpawnTimer = _enemySpawnRepeatRate;
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
            var dirToPlayer = _player.CachedPosition - spawnPos;
            enemy.transform.position = spawnPos;
            enemy.transform.forward = dirToPlayer;
            enemy.ResetData();
            enemy.gameObject.SetActive(true);
            _eventBus.EnemySpawn?.Invoke(enemy);
        }

        private AbstractEnemy GetRandomEnemy()
        {
            var randomIndex = Random.Range(0, _enemyNames.Count);
            var randomName = _enemyNames[randomIndex];
            return _enemyPool.Getitem(randomName);
        }

        private Vector3 GetSpawnPos()
        {
            float spawnAngle = Random.Range(0f, 360f);
            Vector3 spawnDir = Quaternion.Euler(0, spawnAngle, 0) * Vector3.forward;
            return _player.CachedPosition + spawnDir * _spawnRadius;
        }
    }
}


