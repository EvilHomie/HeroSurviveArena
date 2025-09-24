using System;
using UnityEngine;

public interface IPlayerInput
{
    public Action<Vector3> MoveAction { get; set; }
    public Action<Vector3> RotateAction { get; set; }
    public Action StartAtackAction { get; set; }
    public Action EndAtackAction { get; set; }

    public void Subscrube();
    public void Unsubscribe();
}