using DI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace GameSystem
{
    public class EnemiesPool : MonoBehaviour
    {
        private Dictionary<EnemyType, ObjectPool<Enemy>> _poolByType;
        private Dictionary<EnemyType, Enemy> _prefabByType;
        private Container _container;
        private HashSet<Enemy> _activeEnemies;

        [Inject]
        public void Constructor(GameEventBus eventBus, Container container)
        {
            _poolByType = new();
            _prefabByType = new();
            _activeEnemies = new();
            _container = container;
            eventBus.EnemyDie += ReturnEnemy;
        }

        public void AddEnemy(Enemy enemyPF, int startCapacity, int maxCapacity, Transform parent = null, int prewarmCount = 1)
        {
            enemyPF.gameObject.SetActive(false);
            _container.InjectMonoBehaviour(enemyPF);
            _prefabByType.Add(enemyPF.EnemyType, enemyPF);

            var newPool = new ObjectPool<Enemy>(

                   createFunc: () => OnCreateEnemy(enemyPF.EnemyType, parent),
                   actionOnGet: OnGetEnemy,
                   actionOnRelease: OnReturnEnemy,
                   actionOnDestroy: OnDestroyEnemy,
                   defaultCapacity: startCapacity,
                   maxSize: maxCapacity
               );

            _poolByType.Add(enemyPF.EnemyType, newPool);
            PrewarmPool(newPool, prewarmCount);
        }

        public Enemy GetEnemy(EnemyType enemyType)
        {
            return FindPool(enemyType).Get();
        }

        public void ReturnEnemy(Enemy enemy)
        {
            FindPool(enemy.EnemyType).Release(enemy);
            enemy.gameObject.SetActive(false);
        }

        public void ReturnAllActiveEnemies()
        {
            _activeEnemies.RemoveWhere(enemy =>
            {
                ReturnEnemy(enemy);
                return true;
            });
        }

        private Enemy OnCreateEnemy(EnemyType enemyType, Transform parent)
        {
            var prefab = _prefabByType[enemyType];
            var instance = Instantiate(prefab, parent);
            return instance;
        }

        private void OnGetEnemy(Enemy enemy)
        {
            _activeEnemies.Add(enemy);
        }
        private void OnReturnEnemy(Enemy enemy)
        {
            _activeEnemies.Remove(enemy);          
        }

        private void OnDestroyEnemy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }

        private void PrewarmPool(ObjectPool<Enemy> pool, int count)
        {
            List<Enemy> enemies = new();
            for (int i = 0; i < count; i++)
            {
                var instance = pool.Get();
                enemies.Add(instance);
            }
            foreach (Enemy enemy in enemies)
            {
                pool.Release(enemy);
            }
        }

        private ObjectPool<Enemy> FindPool(EnemyType enemyType)
        {
            if (!_poolByType.TryGetValue(enemyType, out var pool))
            {
                throw new Exception($"Не найден пул с {enemyType}");
            }
            return pool;
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
