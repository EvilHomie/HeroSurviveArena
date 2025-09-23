using Enemy;
using UnityEngine;

public class RangedMoveBehavior : AbstractMovementBehavior<Ranged>
{
    public override void Move(Ranged enemy, Vector3 targetPosition, float moveThreshold)
    {
        Vector3 toTarget = targetPosition - enemy.CachedTransform.position;
        float desiredDistance = enemy.AtackDistance;
        float sqrPlayerDistance = toTarget.sqrMagnitude;
        float sqrDesiredDistance = desiredDistance * desiredDistance;

        if (enemy.IsMaintainingDistance)
        {
            if (Mathf.Abs(sqrPlayerDistance - sqrDesiredDistance) > moveThreshold)
            {
                targetPosition -= toTarget.normalized * desiredDistance;
            }
        }
        else if (sqrPlayerDistance <= sqrDesiredDistance)
        {
            return;
        }
        enemy.CachedTransform.position = Vector3.MoveTowards(enemy.CachedTransform.position, targetPosition, enemy.MoveSpeed * Time.deltaTime);
    }
}
