using DI;
using Enemy;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace GameSystem
{
    public class EnemiesPool : MonoBehaviour
    {
        public readonly HashSet<AbstractEnemy> ActiveEnemies = new();

        private readonly Dictionary<string, ObjectPool<AbstractEnemy>> _poolByName = new();
        private readonly Dictionary<string, AbstractEnemy> _prefabByName = new();
        private Container _container;
        private List<AbstractEnemy> _deadEnemies = new();

        GameEventBus _gameEventBus;
        GameFlowSystem _gameFlowSystem;

        [Inject]
        public void Construct(GameEventBus eventBus, Container container, GameFlowSystem gameFlowSystem)
        {
            _container = container;
            
        }

        private void OnEnable()
        {
            Subscribe();
        }
        private void OnDisable()
        {
            Unsubscribe();
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

        public void ReturnEnemy(AbstractEnemy enemy)
        {
            _deadEnemies.Add(enemy);
            enemy.gameObject.SetActive(false);
        }

        public void ReturnAllActiveEnemies()
        {
            foreach (var enemy in ActiveEnemies)
            {
                ReturnEnemy(enemy);
            }

            ActiveEnemies.Clear();
        }

        private AbstractEnemy OnCreateEnemy(string enemyName, Transform parent)
        {
            var prefab = _prefabByName[enemyName];
            var instance = Instantiate(prefab, parent);
            _container.InjectMonoBehaviour(instance);
            return instance;
        }

        private void OnGetEnemy(AbstractEnemy enemy)
        {
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

        private void ClearCollection()
        {
            foreach (var enemy in _deadEnemies)
            {
                FindPool(enemy.EnemyName).Release(enemy);
            }
            _deadEnemies.Clear();
        }

        private void Subscribe()
        {
            _gameEventBus.EnemyDie += ReturnEnemy;
            _gameFlowSystem.PreUpdateTick += ClearCollection;
        }

        private void Unsubscribe()
        {
            _gameEventBus.EnemyDie -= ReturnEnemy;
            _gameFlowSystem.PreUpdateTick -= ClearCollection;
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
