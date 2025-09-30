using UnityEngine;

namespace Projectile
{
    public abstract class ProjectileBase : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public string UsedName { get; private set; }
        [field: SerializeField] public bool AlignToDirection { get; private set; }
        [field: SerializeField] public float DefaultLifeTime { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public ProjectileImpactType ImpactType { get; private set; }
        public float Damage { get; private set; }
        public float Speed { get; private set; }
        public GameObject CachedGameObject { get; private set; }
        public Transform CachedTransform { get; private set; }
        public OwnerType OwnerType { get; private set; }
        public Vector3 Velocity { get; set; }
        public Vector3 CachedPosition { get; set; }
        public float LeftLifeTime { get; set; }
        public abstract ProjectileType Type { get; }

        public void Init()
        {
            CachedTransform = transform;
            CachedGameObject = gameObject;
        }

        public virtual void Config(Transform target, float speed, OwnerType owner, float damage, Vector3 position)
        {
            CachedPosition = position;
            CachedTransform.position = position;
            var direction = target.position - position;
            Velocity = direction.normalized * speed;
            Damage = damage;
            Speed = speed;
            OwnerType = owner;
            LeftLifeTime = DefaultLifeTime;

            if (AlignToDirection)
            {
                CachedTransform.forward = direction;
            }
        }
    }
}