using UnityEngine;

namespace Enemy
{
    public class KamikadzeMovement : EnemyMovementBase<Kamikadze>
    {
        public override void Move(Kamikadze enemy, Player target)
        {
            enemy.CachedTransform.position = Vector3.MoveTowards(enemy.CachedPosition, target.CachedPosition, enemy.MoveSpeed * Time.deltaTime);
            enemy.CachedPosition = enemy.CachedTransform.position;
        }
    }
}