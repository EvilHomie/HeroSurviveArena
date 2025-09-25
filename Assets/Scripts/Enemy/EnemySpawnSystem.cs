using DI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystem
{
    public class EnemySpawnSystem : MonoBehaviour
    {
        private GameFlowSystem _gameFlowSystem;
        private EnemiesPool _enemyPool;
        private Player _player;
        private GameEventBus _eventBus;
        private float _enemySpawnRepeatRate;
        private float _enemySpawnTimer;
        private float _spawnRadius;

        [Inject]
        public void Construct(GameFlowSystem gameFlowSystem, Config config, EnemiesPool enemiesPool, Player player, GameEventBus eventBus)
        {
            _gameFlowSystem = gameFlowSystem;
            _enemySpawnRepeatRate = config.EnemySpawnRepeatRate;
            _enemyPool = enemiesPool;
            _player = player;
            _spawnRadius = config.EnemySpawnRadius;
            _eventBus = eventBus;            
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
            var enemy = _enemyPool.GetRandomEnemy();
            var spawnPos = GetSpawnPos();
            var dirToPlayer = _player.CachedPosition - spawnPos;
            enemy.CachedTransform.position = spawnPos;
            enemy.CachedTransform.forward = dirToPlayer;
            enemy.ResetData();
            enemy.gameObject.SetActive(true);
            _eventBus.EnemySpawn?.Invoke(enemy);
        }

        private Vector3 GetSpawnPos() // возвращает позицию на окружности от игрока 
        {
            float spawnAngle = Random.Range(0f, 360f);
            Vector3 spawnDir = Quaternion.Euler(0, spawnAngle, 0) * Vector3.forward;
            return _player.CachedPosition + spawnDir * _spawnRadius;
        }
    }
}