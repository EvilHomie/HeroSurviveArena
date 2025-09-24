using UnityEngine;

namespace Enemy
{
    public abstract class AbstractMovementBehavior<TEnemy> : IMovementBehavior<TEnemy> where TEnemy : AbstractEnemy
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

        void IMovementBehaviorBase.Move(AbstractEnemy enemy, Player target, float sqrMoveThreshold)
            => Move((TEnemy)enemy, target, sqrMoveThreshold);
        void IMovementBehaviorBase.RotateTowards(AbstractEnemy enemy, Player target)
            => RotateTowards((TEnemy)enemy, target);
        void IMovementBehaviorBase.MoveAndRotate(AbstractEnemy enemy, Player target, float sqrMoveThreshold)
            => MoveAndRotate((TEnemy)enemy, target, sqrMoveThreshold);
    }

    public interface IMovementBehaviorBase
    {
        void Move(AbstractEnemy enemy, Player target, float sqrMoveThreshold);
        void RotateTowards(AbstractEnemy enemy, Player target);
        void MoveAndRotate(AbstractEnemy enemy, Player target, float sqrMoveThreshold);
    }

    public interface IMovementBehavior<TEnemy> : IMovementBehaviorBase where TEnemy : AbstractEnemy
    {
        void Move(TEnemy enemy, Player target, float sqrMoveThreshold);
        void RotateTowards(TEnemy enemy, Player target);
        void MoveAndRotate(TEnemy enemy, Player target, float sqrMoveThreshold);
    }
}