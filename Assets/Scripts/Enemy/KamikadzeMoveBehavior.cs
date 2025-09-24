using UnityEngine;

namespace Enemy
{
    public class KamikadzeMoveBehavior : AbstractMovementBehavior<Kamikadze>
    {
        public override void Move(Kamikadze enemy, Player target, float moveThreshold)
        {
            enemy.CachedTransform.position = Vector3.MoveTowards(enemy.CachedPosition, target.CachedPosition, enemy.MoveSpeed * Time.deltaTime);
            enemy.CachedPosition = enemy.CachedTransform.position;
        }
    }
}