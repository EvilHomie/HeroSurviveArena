using DI;
using System;
using UnityEngine;

namespace Enemy
{
    public abstract class AbstractEnemy : MonoBehaviour, IPoolable
    {
        [SerializeField] float _defaultHealthPoint;
        [field: SerializeField] public float AtackDistance { get; private set; }
        [field: SerializeField] public float AttackDamage { get; private set; }
        [field: SerializeField] public float ContactDamage { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }

        //public Vector3 CachedPosition { get; set; }
        //public float CurrentHealthPoint { get; set; }
        //public Transform CachedTransform { get; private set; }
        //public Type EnemyType { get; private set; }
        //
        //public bool IsDead {  get; set; }



        [field: SerializeField] public string UsedName { get; private set; }
        public GameObject CachedGameObject { get; private set; }
        public Transform CachedTransform { get; private set; }
        public Type CashedType { get; private set; }


        public float CurrentHealthPoint { get; set; }
        public Vector3 CachedPosition { get; set; }
        public bool InAttackRange { get; set; }

        public void Init()
        {
            CachedTransform = transform;
            CachedGameObject = gameObject;
            CashedType = GetType();
        }

        public void ResetData()
        {
            CurrentHealthPoint = _defaultHealthPoint;
            CachedPosition = transform.position;
            InAttackRange = false;
        }
    }
}