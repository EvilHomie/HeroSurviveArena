using DI;
using System;
using UnityEngine;

namespace Enemy
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [SerializeField] float _defaultHealthPoint;
        [field: SerializeField] public string EnemyName { get; private set; }
        [field: SerializeField] public float AtackDistance { get; private set; }
        [field: SerializeField] public float AttackDamage { get; private set; }
        [field: SerializeField] public float ContactDamage { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }

        public bool IsDead { get; set; }
        public Vector3 CachedPosition { get; set; }
        public float CurrentHealthPoint { get; set; }
        public Transform CachedTransform { get; private set; }
        public Type EnemyType { get; private set; }
        public bool InAttackRange {  get; set; }


        [Inject]
        public void Construct()
        {
            CachedTransform = transform;            
            EnemyType = GetType();
        }

        public void ResetData()
        {
            CurrentHealthPoint = _defaultHealthPoint;
            CachedPosition = transform.position;
            IsDead = false;
        }
    }
}