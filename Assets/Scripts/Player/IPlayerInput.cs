using System;
using UnityEngine;

public interface IPlayerInput
{
    public event Action<Vector3> MoveAction;
    public event Action<Vector3> RotateAction;
    public event Action StartAtackAction;
    public event Action EndAtackAction;
}