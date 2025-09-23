using Enemy;
using TMPro;
using UnityEngine;

public abstract class AbstractMovementBehavior<TEnemy> : IMovementBehavior<TEnemy> where TEnemy : AbstractEnemy
{
    public abstract void Move(TEnemy enemy, Vector3 targetPosition, float moveThreshold);

    public void RotateTowards(TEnemy enemy, Vector3 toTarget)
    {
        if (toTarget.sqrMagnitude > 0.0001f)
        {
            enemy.CachedTransform.rotation = Quaternion.LookRotation(toTarget);
        }
    }

    public void MoveAndRotate(TEnemy enemy, Vector3 targetPosition, float moveThreshold)
    {
        Move(enemy, targetPosition, moveThreshold);
        Vector3 toTarget = targetPosition - enemy.CachedTransform.position;
        RotateTowards(enemy, toTarget);
    }

    void IMovementBehaviorBase.Move(AbstractEnemy enemy, Vector3 targetPosition, float moveThreshold)
        => Move((TEnemy)enemy, targetPosition, moveThreshold);
    void IMovementBehaviorBase.RotateTowards(AbstractEnemy enemy, Vector3 targetPosition)
        => RotateTowards((TEnemy)enemy, targetPosition);
    void IMovementBehaviorBase.MoveAndRotate(AbstractEnemy enemy, Vector3 targetPosition, float moveThreshold)
        => MoveAndRotate((TEnemy)enemy, targetPosition, moveThreshold);
}

public interface IMovementBehaviorBase
{
    void Move(AbstractEnemy enemy, Vector3 targetPosition, float moveThreshold);
    void RotateTowards(AbstractEnemy enemy, Vector3 targetPosition);
    void MoveAndRotate(AbstractEnemy enemy, Vector3 targetPosition, float moveThreshold);
}

public interface IMovementBehavior<TEnemy> : IMovementBehaviorBase where TEnemy : AbstractEnemy
{
    void Move(TEnemy enemy, Vector3 targetPosition, float moveThreshold);
    void RotateTowards(TEnemy enemy, Vector3 targetPosition);
    void MoveAndRotate(TEnemy enemy, Vector3 targetPosition, float moveThreshold);
}
