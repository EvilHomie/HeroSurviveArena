using Projectile;
using UnityEngine;

namespace Enemy
{
    public class EnemyShooter : EnemyBase
    {
        [field: SerializeField] public ProjectileBase ShootProjectile { get; private set; }
        [field: SerializeField] public float MaintainingDistance { get; private set; }
        [field: SerializeField] public bool CanAttackWhileMoving { get; private set; }
        [field: SerializeField] public float AttackRate { get; private set; }
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
        [field: SerializeField] public float ProjectileLifeTime { get; private set; }
        public float ReloadTimer { get; set; }
        public bool IsReloaded {  get; set; }
    }
}