using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [field: SerializeField] public string UsedName { get ; private set; }
    public Vector3 CachedPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float CurrentHealthPoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Transform CachedTransform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Type CashedType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public GameObject CachedGameObject => throw new NotImplementedException();

    public void Init()
    {
        //CachedTransform = transform;
        //EnemyType = GetType();
    }

    public void ResetData()
    {
        //CurrentHealthPoint = _defaultHealthPoint;
        //CachedPosition = transform.position;
        //IsDead = false;
    }
}
