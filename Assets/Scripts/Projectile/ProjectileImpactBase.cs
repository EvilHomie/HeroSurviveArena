using Enemy;
using Projectile;
using System;
using UnityEngine;

public abstract class ProjectileImpactBase<T> where T : IDamageable
{
    protected Collider[] Hits = new Collider[10];
    protected LayerMask ImpactLayer;
    protected Action<ProjectileBase> DieAction;
    protected Action<T> KillAction;

    public ProjectileImpactBase(int maxImpactCount, LayerMask impactLayer, Action<ProjectileBase> dieAction, Action<T> killAction)
    {
        Hits = new Collider[maxImpactCount];
        ImpactLayer = impactLayer;
        DieAction = dieAction;
        KillAction = killAction;
    }
    public abstract void HandleImpact(ProjectileBase projectile);
}
