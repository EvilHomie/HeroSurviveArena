using DI;
using Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace GameSystem
{
    public class EnemiesPool : MonoBehaviour
    {
        public readonly HashSet<AbstractEnemy> ActiveEnemies = new();

        private readonly Dictionary<string, ObjectPool<AbstractEnemy>> _poolByName = new();
        private readonly Dictionary<string, AbstractEnemy> _prefabByName = new();
        private readonly List<AbstractEnemy> _deadEnemies = new();

        GameEventBus _gameEventBus;
        GameFlowSystem _gameFlowSystem;

        [Inject]
        public void Construct(GameEventBus eventBus, GameFlowSystem gameFlowSystem)
        {
            _gameEventBus = eventBus;
            _gameFlowSystem = gameFlowSystem;
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
            _gameEventBus.EnemyDie += OnEnemyDie;
            _gameEventBus.ChangeGameState += OnChangeGameState;
            _gameFlowSystem.UpdateTick += ReturnDeadToPool;
        }

        private void Unsubscribe()
        {
            _gameEventBus.EnemyDie -= OnEnemyDie;
            _gameEventBus.ChangeGameState -= OnChangeGameState;
            _gameFlowSystem.UpdateTick -= ReturnDeadToPool;
        }

        public void AddEnemy(AbstractEnemy enemyPF, int startCapacity, int maxCapacity, Transform parent = null, int prewarmCount = 1)
        {
            enemyPF.gameObject.SetActive(false);
            _prefabByName.Add(enemyPF.EnemyName, enemyPF);

            var newPool = new ObjectPool<AbstractEnemy>(

                   createFunc: () => OnCreateEnemy(enemyPF.EnemyName, parent),
                   actionOnGet: OnGetEnemy,
                   actionOnRelease: OnReturnEnemy,
                   actionOnDestroy: OnDestroyEnemy,
                   defaultCapacity: startCapacity,
                   maxSize: maxCapacity
               );

            _poolByName.Add(enemyPF.EnemyName, newPool);
            PrewarmPool(newPool, prewarmCount);
        }

        public AbstractEnemy GetEnemy(string EnemyName)
        {
            return FindPool(EnemyName).Get();
        }       

        private AbstractEnemy OnCreateEnemy(string enemyName, Transform parent)
        {
            var prefab = _prefabByName[enemyName];
            var instance = Instantiate(prefab, parent);
            instance.Init();
            return instance;
        }

        private void OnGetEnemy(AbstractEnemy enemy)
        {
            enemy.ResetData();
            ActiveEnemies.Add(enemy);
        }
        private void OnReturnEnemy(AbstractEnemy enemy)
        {
            ActiveEnemies.Remove(enemy);
        }

        private void OnDestroyEnemy(AbstractEnemy enemy)
        {
            Destroy(enemy.gameObject);
        }

        private void PrewarmPool(ObjectPool<AbstractEnemy> pool, int count)
        {
            List<AbstractEnemy> enemies = new();

            for (int i = 0; i < count; i++)
            {
                var instance = pool.Get();
                enemies.Add(instance);
            }

            foreach (AbstractEnemy enemy in enemies)
            {
                pool.Release(enemy);
            }
        }

        private ObjectPool<AbstractEnemy> FindPool(string enemyName)
        {
            if (!_poolByName.TryGetValue(enemyName, out var pool))
            {
                throw new Exception($"Не найден пул с {enemyName}");
            }

            return pool;
        }

        private void ReturnAllActiveEnemies()
        {
            foreach (var enemy in ActiveEnemies)
            {
                OnEnemyDie(enemy);
            }

            ReturnDeadToPool();
        }

        private void OnEnemyDie(AbstractEnemy enemy)
        {
            _deadEnemies.Add(enemy);
            enemy.gameObject.SetActive(false);
        }

        private void ReturnDeadToPool()
        {
            foreach (var enemy in _deadEnemies)
            {
                FindPool(enemy.EnemyName).Release(enemy);
                ActiveEnemies.Remove(enemy);
            }

            _deadEnemies.Clear();
        }

        private void OnChangeGameState(GameState   gameState )
        {
            if (gameState == GameState.GameOver || gameState == GameState.Victory)
            {
                ReturnAllActiveEnemies();
            }
        }
    }
}



/* просто заметка
* var index = Random.Range(0, list.Count);
var item = list[index];

// Меняем местами с последним элементом
list[index] = list[list.Count - 1];

// Удаляем последний элемент
list.RemoveAt(list.Count - 1);
*/
