using UnityEngine;

namespace Enemy
{
    public abstract class EnemyMovementBase<TEnemy> where TEnemy : EnemyBase
    {
        public abstract void Move(TEnemy enemy, Player target);

        public void RotateTowards(TEnemy enemy, Player target)
        {
            Vector3 toTarget = target.CachedPosition - enemy.CachedPosition;

            if (toTarget.sqrMagnitude > 0.0001f) // просто проверка на совпадение позиций
            {
                enemy.CachedTransform.rotation = Quaternion.LookRotation(toTarget);
            }
        }

        public void MoveAndRotate(TEnemy enemy, Player target)
        {
            Move(enemy, target);
            RotateTowards(enemy, target);
        }
    }
}