using System;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyBase : MonoBehaviour, IPoolable, IDamageable
    {
        [field: SerializeField] public string UsedName { get; private set; }
        [SerializeField] float _defaultHealthPoint;
        public float CurrentHealthPoint { get; set; }
        public Vector3 CachedPosition { get; set; }
        public bool InAttackRange { get; set; }
        [field: SerializeField] public float AtackDistance { get; private set; }
        [field: SerializeField] public float AttackDamage { get; private set; }
        [field: SerializeField] public float ContactDamage { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        public GameObject CachedGameObject { get; private set; }
        public Transform CachedTransform { get; private set; }
        public abstract EnemyType Type { get; }


        public void Init()
        {
            CachedTransform = transform;
            CachedGameObject = gameObject;
        }

        public void ResetData()
        {
            CurrentHealthPoint = _defaultHealthPoint;
            CachedPosition = CachedTransform.position;
            InAttackRange = false;
        }
    }
}