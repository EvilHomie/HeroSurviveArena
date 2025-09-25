using Enemy;
using Projectile;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Scriptable Objects/Config")]
public class Config : ScriptableObject
{
    [field: SerializeField] public List<EnemyBase> Enemies { get; private set; }
    [field: SerializeField] public float EnemySpawnRepeatRate { get; private set; }
    [field: SerializeField] public float EnemySpawnRadius { get; private set; }
    [field: SerializeField] public int EnemyPoolStartCapacity { get; private set; }
    [field: SerializeField] public int EnemyPoolMaxCapacity { get; private set; }
    [field: SerializeField] public int EnemyPoolPrewarmCount { get; private set; }

    [field: SerializeField] public List<ProjectileBase> Projectiles { get; private set; }
    [field: SerializeField] public int ProjectilePoolStartCapacity { get; private set; }
    [field: SerializeField] public int ProjectilePoolMaxCapacity { get; private set; }
    [field: SerializeField] public int ProjectilePoolPrewarmCount { get; private set; }


}