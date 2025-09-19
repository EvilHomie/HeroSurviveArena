using DI;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //[SerializeField] float _defaultHealthPoint;
    //[SerializeField] float _defaultMoveSpeed;
    //[SerializeField] float _defaultDamage;

    [SerializeField] EnemyType _enemyType;

    public EnemyType EnemyType => _enemyType;

    public void ResetData()
    {

    }

    //private void OnTriggerEnter(Collider other)
    //{

    //}
}
