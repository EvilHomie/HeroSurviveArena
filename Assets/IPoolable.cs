using System;
using UnityEngine;

public interface IPoolable
{
    public string UsedName { get; }
    public GameObject CachedGameObject { get; }
    public Transform CachedTransform { get; }
    public Vector3 CachedPosition { get; }
    public Type CashedType { get; }

    public void Init();
    public void ResetData();
}
