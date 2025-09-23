using Enemy;
using UnityEngine;

public class KamikadzeMoveBehavior : AbstractMovementBehavior<Kamikadze>
{
    public override void Move(Kamikadze enemy, Vector3 targetPosition, float moveThreshold)
    {
        enemy.CachedTransform.position = Vector3.MoveTowards(enemy.CachedTransform.position, targetPosition, enemy.MoveSpeed * Time.deltaTime);
    }
}
