using Projectile;
using System;
using UnityEngine;

public class ProjectileHoming : ProjectileBase
{
    public Transform TargetTransform {  get; private set; }

    public float Speed { get; private set; }

    public override void Config(Transform target, float speed, Type owner, float damage, Vector3 position)
    {
        base.Config(target, speed, owner, damage, position);
        TargetTransform = target;
        Speed = speed;
    }
}
