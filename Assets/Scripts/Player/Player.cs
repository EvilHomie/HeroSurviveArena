using DI;
using GameSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 CachedPosition { get; set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }

    [SerializeField] float _defaultHealthPoint;

    public float CurrentHealthPoint { get; set; }
    public Transform CachedTransform { get; private set; }


    [Inject]
    public void Construct()
    {
        CachedTransform = transform;
        CachedPosition = transform.position;
    }

    public void ResetData()
    {
        CurrentHealthPoint = _defaultHealthPoint;
        CachedPosition = transform.position;
        CachedPosition = Vector3.zero;
        transform.position = CachedPosition;
    }
}
