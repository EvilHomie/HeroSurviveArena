using System;
using UnityEngine;

public interface IPoolable
{
    public string UsedName { get; }
    public GameObject CachedGameObject { get; }
    public void Init();
}
