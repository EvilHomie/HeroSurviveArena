using Enemy;
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
        float moveDistance = projectile.Speed * Time.deltaTime;
        Ray ray = new(projectile.CachedPosition, projectile.Velocity);

        if (Physics.Raycast(ray, out RaycastHit hit, moveDistance, ImpactLayer))
        {
            if (hit.collider.TryGetComponent(out T hitedObj))
            {
                hitedObj.CurrentHealthPoint -= projectile.Damage;

                if (hitedObj.CurrentHealthPoint < 0)
                {
                    KillAction?.Invoke(hitedObj);
                }

                DieAction?.Invoke(projectile);
            }
        }
    }
}
