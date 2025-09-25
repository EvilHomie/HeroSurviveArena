using DI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace GameSystem
{
    public abstract class AbstractPool<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
    {
        public readonly HashSet<T> ItemsInUse = new();
        protected GameEventBus _gameEventBus;
        protected GameFlowSystem _gameFlowSystem;

        private readonly Dictionary<string, ObjectPool<T>> _poolByName = new();
        private readonly Dictionary<string, T> _prefabByName = new();
        private readonly List<T> _inactiveItems = new();

        protected private Config _config;

        [Inject]
        public void Construct(GameEventBus eventBus, GameFlowSystem gameFlowSystem, Config config)
        {
            _gameEventBus = eventBus;
            _gameFlowSystem = gameFlowSystem;
            _config = config;
        }

        private void OnEnable()
        {
            Subscribe();
        }
        private void OnDisable()
        {
            Unsubscribe();
        }      

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

        protected void CreateItemPool(T prefab, int startCapacity, int maxCapacity, Transform parent = null, int prewarmCount = 1) 
        {
            prefab.gameObject.SetActive(false);
            _prefabByName.Add(prefab.UsedName, prefab);

            var newPool = new ObjectPool<T>(

                   createFunc: () => OnCreate(prefab.UsedName, parent),
                   actionOnGet: OnGet,
                   actionOnRelease: OnRelease,
                   actionOnDestroy: OnDestroyItem,
                   defaultCapacity: startCapacity,
                   maxSize: maxCapacity
               );

            _poolByName.Add(prefab.UsedName, newPool);
            PrewarmPool(newPool, prewarmCount);
        }

        public T Getitem(string itemName)
        {
            return FindPool(itemName).Get();
        }

        protected void OnItemDeactivated(T item)
        {
            _inactiveItems.Add(item);
            item.CachedGameObject.SetActive(false);
        }

        protected void ReleaseAll()
        {
            foreach (var enemy in ItemsInUse)
            {
                OnItemDeactivated(enemy);
            }

            ReleaseInactive();
        }

        protected void ReleaseInactive()
        {
            foreach (var item in _inactiveItems)
            {
                FindPool(item.UsedName).Release(item);
                ItemsInUse.Remove(item);
            }

            _inactiveItems.Clear();
        }

        private T OnCreate(string itemName, Transform parent)
        {
            var prefab = _prefabByName[itemName];
            var instance = Instantiate(prefab, parent);
            instance.Init();
            return instance;
        }

        private void OnGet(T item)
        {
            ItemsInUse.Add(item);
        }
        private void OnRelease(T item)
        {
            ItemsInUse.Remove(item);
        }

        private void OnDestroyItem(T item)
        {
            Destroy(item.CachedGameObject);
        }

        private void PrewarmPool(ObjectPool<T> pool, int count)
        {
            List<T> items = new();

            for (int i = 0; i < count; i++)
            {
                var instance = pool.Get();
                items.Add(instance);
            }

            foreach (T item in items)
            {
                pool.Release(item);
            }
        }

        private ObjectPool<T> FindPool(string name)
        {
            if (!_poolByName.TryGetValue(name, out var pool))
            {
                throw new Exception($"Не найден пул с {name}");
            }

            return pool;
        }  
    }
}
