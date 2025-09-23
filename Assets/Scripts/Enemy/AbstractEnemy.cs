using DI;
using GameSystem;
using System;
using UnityEngine;

namespace Enemy
{
    public abstract class AbstractEnemy : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public string EnemyName { get; private set; }
        [field: SerializeField] public float AtackDistance { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float ContactDamage { get; private set; }

        [SerializeField] float _defaultHealthPoint;
        private GameEventBus _eventBus;

        public bool InAtackRange { get; set; }
        public float CurrentHealthPoint { get; private set; }
        public Transform CachedTransform { get; private set; }

        public Type EnemyType { get; private set; }


        [Inject]
        public void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
            CachedTransform = transform;
            EnemyType = GetType();
        }

        public virtual void ResetData()
        {
            CurrentHealthPoint = _defaultHealthPoint;
            InAtackRange = false;
        }

        public void Damage(float value)
        {
            CurrentHealthPoint -= value;
            if (CurrentHealthPoint <= 0)
            {
                _eventBus.EnemyDie?.Invoke(this);
            }
        }

        public abstract void Atack();
    }
}