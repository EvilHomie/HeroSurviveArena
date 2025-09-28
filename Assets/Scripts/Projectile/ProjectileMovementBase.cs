using UnityEngine;

namespace Projectile
{
    public abstract class ProjectileMovementBase<TProjectile> where TProjectile : ProjectileBase
    {
        public virtual void Move(TProjectile projectile)
        {
            projectile.CachedTransform.position += projectile.Velocity * Time.deltaTime;
            projectile.CachedPosition = projectile.CachedTransform.position;
        }
    }
}