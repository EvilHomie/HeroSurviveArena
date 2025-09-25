using UnityEngine;

namespace Enemy
{
    public abstract class MovementBehaviorBase<TEnemy> : IMovementBehavior<TEnemy> where TEnemy : EnemyBase
    {
        public abstract void Move(TEnemy enemy, Player target, float moveThreshold);

        public void RotateTowards(TEnemy enemy, Player target)
        {
            Vector3 toTarget = target.CachedPosition - enemy.CachedPosition;

            if (toTarget.sqrMagnitude > 0.0001f) // просто проверка на совпадение позиций
            {
                enemy.CachedTransform.rotation = Quaternion.LookRotation(toTarget);
            }
        }

        public void MoveAndRotate(TEnemy enemy, Player target, float moveThreshold)
        {
            Move(enemy, target, moveThreshold);
            RotateTowards(enemy, target);
        }

        void IMovementBehaviorBase.Move(EnemyBase enemy, Player target, float sqrMoveThreshold)
            => Move((TEnemy)enemy, target, sqrMoveThreshold);
        void IMovementBehaviorBase.RotateTowards(EnemyBase enemy, Player target)
            => RotateTowards((TEnemy)enemy, target);
        void IMovementBehaviorBase.MoveAndRotate(EnemyBase enemy, Player target, float sqrMoveThreshold)
            => MoveAndRotate((TEnemy)enemy, target, sqrMoveThreshold);
    }

    public interface IMovementBehaviorBase
    {
        void Move(EnemyBase enemy, Player target, float sqrMoveThreshold);
        void RotateTowards(EnemyBase enemy, Player target);
        void MoveAndRotate(EnemyBase enemy, Player target, float sqrMoveThreshold);
    }

    public interface IMovementBehavior<TEnemy> : IMovementBehaviorBase where TEnemy : EnemyBase
    {
        void Move(TEnemy enemy, Player target, float sqrMoveThreshold);
        void RotateTowards(TEnemy enemy, Player target);
        void MoveAndRotate(TEnemy enemy, Player target, float sqrMoveThreshold);
    }
}