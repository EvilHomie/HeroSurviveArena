using DI;
using GameSystem;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{    
    [field: SerializeField] public EnemyType EnemyType { get; private set; }
    [field: SerializeField] public float DesiredDistance { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float ContactDamage { get; private set; }

    [SerializeField] float _defaultHealthPoint;
    private GameEventBus _eventBus;


    public float CurentHealthPoint { get; private set; }


    [Inject]
    public void Constructor(GameEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void ResetData()
    {
        CurentHealthPoint = _defaultHealthPoint;
    }

    public void Damage(float value)
    {
        CurentHealthPoint -= value;
        if (CurentHealthPoint <= 0)
        {
            _eventBus.EnemyDie(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
