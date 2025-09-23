using Enemy;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Scriptable Objects/Config")]
public class Config : ScriptableObject
{
    [field: SerializeField] public float PlayerSpeed { get; private set; }
    [field: SerializeField] public List<AbstractEnemy> Enemies { get; private set; }
    [field: SerializeField] public float EnemySpawnRepeatRate { get; private set; }
    [field: SerializeField] public float EnemySpawnRadius { get; private set; }
    [field: SerializeField] public int EnemyPoolStartCapacity { get; private set; }
    [field: SerializeField] public int EnemyPoolMaxCapacity { get; private set; }
    [field: SerializeField] public int EnemyPoolPrewarmCount { get; private set; }

}