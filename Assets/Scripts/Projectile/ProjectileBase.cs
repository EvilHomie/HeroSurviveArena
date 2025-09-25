using System;
using UnityEngine;

namespace Projectile
{
    public abstract class ProjectileBase : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public string UsedName { get; private set; }
        [field: SerializeField] public bool AlignToDirection { get; private set; }
        public float Damage { get; private set; }
        public GameObject CachedGameObject { get; private set; }
        public Transform CachedTransform { get; private set; }
        public Type CashedType { get; private set; }
        public Type CashedOwner { get; private set; }
        public Vector3 Velocity { get; set; }
        public Vector3 CachedPosition { get; set; }

        public void Init()
        {
            CachedTransform = transform;
            CachedGameObject = gameObject;
            CashedType = GetType();
        }

        public virtual void Config(Transform target, float speed, Type owner, float damage, Vector3 position)
        {
            CachedPosition = position;
            CachedTransform.position = position;
            CashedOwner = owner;
            var direction = target.position - position;
            Velocity = direction.normalized * speed;
            Damage = damage;            

            if (AlignToDirection)
            {
                CachedTransform.forward = direction;
            }
        }
    }
}