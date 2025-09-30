using Projectile;
using System;
using UnityEngine;

public class ProjectileSimpleImpact<T> : ProjectileImpactBase<T> where T : IDamageable
{
    public ProjectileSimpleImpact(int maxImpactCount, LayerMask impactLayer, Action<ProjectileBase> dieAction, Action<T> killAction) : base(maxImpactCount, impactLayer, dieAction, killAction)
    {
    }

    public override void HandleImpact(ProjectileBase projectile)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(projectile.CachedPosition, projectile.Radius, Hits, ImpactLayer);

        if (hitCount > 0 && Hits[0].TryGetComponent(out T target))
        {
            target.CurrentHealthPoint -= projectile.Damage;

            if (target.CurrentHealthPoint <= 0)
            {
                KillAction?.Invoke(target);
            }

            DieAction?.Invoke(projectile);
        }
    }
}
