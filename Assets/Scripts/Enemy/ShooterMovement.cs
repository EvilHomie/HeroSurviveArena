using UnityEngine;

namespace Enemy
{
    public class ShooterMovement : EnemyMovementBase<Shooter>
    {
        private readonly float _sqrMoveThreshold;

        public ShooterMovement(float moveThreshold)
        {
            _sqrMoveThreshold = moveThreshold * moveThreshold;
        }

        public override void Move(Shooter enemy, Player target)
        {
            if (enemy.MaintainingDistance == 0)
            {
                MoveToAttackRange(enemy, target);
            }
            else
            {
                MaintainDistance(enemy, target);
            }
        }

        private void MaintainDistance(Shooter enemy, Player target)
        {
            Vector3 toTarget = target.CachedPosition - enemy.CachedTransform.position;
            float sqrPlayerDistance = toTarget.sqrMagnitude;
            float sqrMaintainingDistance = enemy.MaintainingDistance * enemy.MaintainingDistance;

            if (Mathf.Abs(sqrPlayerDistance - sqrMaintainingDistance) > _sqrMoveThreshold)
            {
                Vector3 toPosition = target.CachedPosition - toTarget.normalized * enemy.MaintainingDistance;
                enemy.CachedTransform.position = Vector3.MoveTowards(enemy.CachedTransform.position, toPosition, enemy.MoveSpeed * Time.deltaTime);
                enemy.CachedPosition = enemy.CachedTransform.position;
            }
        }

        private void MoveToAttackRange(Shooter enemy, Player target)
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