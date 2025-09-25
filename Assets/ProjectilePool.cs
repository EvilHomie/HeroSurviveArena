using DI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace GameSystem
{
    public class ProjectilePool : MonoBehaviour
    {
        public readonly HashSet<Projectile> ActiveItems = new();

        private readonly Dictionary<string, ObjectPool<Projectile>> _poolByName = new();
        private readonly Dictionary<string, Projectile> _prefabByName = new();
        private readonly List<Projectile> _inactiveItems = new();

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
            _gameEventBus.ChangeGameState += OnChangeGameState;
            _gameFlowSystem.UpdateTick += ReleaseToPool;
        }

        private void Unsubscribe()
        {
            _gameEventBus.ChangeGameState -= OnChangeGameState;
            _gameFlowSystem.UpdateTick -= ReleaseToPool;
        }

        public void CreateItemPool(Projectile projectilePF, int startCapacity, int maxCapacity, Transform parent = null, int prewarmCount = 1)
        {
            projectilePF.gameObject.SetActive(false);
            _prefabByName.Add(projectilePF.UsedName, projectilePF);

            var newPool = new ObjectPool<Projectile>(

                   createFunc: () => OnCreate(projectilePF.UsedName, parent),
                   actionOnGet: OnGet,
                   actionOnRelease: OnRelease,
                   actionOnDestroy: OnDestroyItem,
                   defaultCapacity: startCapacity,
                   maxSize: maxCapacity
               );

            _poolByName.Add(projectilePF.UsedName, newPool);
            PrewarmPool(newPool, prewarmCount);
        }

        public Projectile GetEnemy(string projectileName)
        {
            return FindPool(projectileName).Get();
        }

        private Projectile OnCreate(string itemName, Transform parent)
        {
            var prefab = _prefabByName[itemName];
            var instance = Instantiate(prefab, parent);
            instance.Init();
            return instance;
        }

        private void OnGet(Projectile item)
        {
            item.ResetData();
            ActiveItems.Add(item);
        }
        private void OnRelease(Projectile item)
        {
            ActiveItems.Remove(item);
        }

        private void OnDestroyItem(Projectile item)
        {
            Destroy(item.gameObject);
        }

        private void PrewarmPool(ObjectPool<Projectile> pool, int count)
        {
            List<Projectile> items = new();

            for (int i = 0; i < count; i++)
            {
                var instance = pool.Get();
                items.Add(instance);
            }

            foreach (Projectile item in items)
            {
                pool.Release(item);
            }
        }

        private ObjectPool<Projectile> FindPool(string name)
        {
            if (!_poolByName.TryGetValue(name, out var pool))
            {
                throw new Exception($"Не найден пул с {name}");
            }

            return pool;
        }

        private void ReleaseInactive()
        {
            foreach (var enemy in ActiveItems)
            {
                OnItemDeactivated(enemy);
            }

            ReleaseToPool();
        }

        private void OnItemDeactivated(Projectile item)
        {
            _inactiveItems.Add(item);
            item.gameObject.SetActive(false);
        }

        private void ReleaseToPool()
        {
            foreach (var item in _inactiveItems)
            {
                FindPool(item.UsedName).Release(item);
                ActiveItems.Remove(item);
            }

            _inactiveItems.Clear();
        }

        private void OnChangeGameState(GameState gameState)
        {
            if (gameState == GameState.GameOver || gameState == GameState.Victory)
            {
                ReleaseInactive();
            }
        }
    }
}
