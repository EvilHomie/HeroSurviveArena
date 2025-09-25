using UnityEngine;

namespace Enemy
{
    public class RangedMoveBehavior : AbstractMovementBehavior<EnemyShooter>
    {
        public override void Move(EnemyShooter enemy, Player target, float sqrMoveThreshold)
        {
            if (enemy.MaintainingDistance == 0)
            {
                MoveToAttackRange(enemy, target);
            }
            else
            {
                MaintainDistance(enemy, target, sqrMoveThreshold);
            }
        }

        private void MaintainDistance(EnemyShooter enemy, Player target, float sqrMoveThreshold)
        {
            Vector3 toTarget = target.CachedPosition - enemy.CachedTransform.position;
            float sqrPlayerDistance = toTarget.sqrMagnitude;
            float sqrMaintainingDistance = enemy.MaintainingDistance * enemy.MaintainingDistance;

            if (Mathf.Abs(sqrPlayerDistance - sqrMaintainingDistance) > sqrMoveThreshold)
            {
                Vector3 toPosition = target.CachedPosition - toTarget.normalized * enemy.MaintainingDistance;
                enemy.CachedTransform.position = Vector3.MoveTowards(enemy.CachedTransform.position, toPosition, enemy.MoveSpeed * Time.deltaTime);
                enemy.CachedPosition = enemy.CachedTransform.position;
            }
        }

        private void MoveToAttackRange(EnemyShooter enemy, Player target)
        {
            Vector3 toTarget = target.CachedPosition - enemy.CachedTransform.position;
            float sqrPlayerDistance = toTarget.sqrMagnitude;
            float sqrAtackDistance = enemy.AtackDistance * enemy.AtackDistance;

            if (sqrPlayerDistance > sqrAtackDistance)
            {
                enemy.CachedTransform.position = Vector3.MoveTowards(enemy.CachedTransform.position, target.CachedPosition, enemy.MoveSpeed * Time.deltaTime);
                enemy.CachedPosition = enemy.CachedTransform.position;
            }
        }
    }
}