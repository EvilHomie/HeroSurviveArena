using GameSystem;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyAttackBase<TEnemy> where TEnemy : EnemyBase
    {
        protected GameEventBus EventBus;

        public EnemyAttackBase(GameEventBus gameEventBus)
        {
            EventBus = gameEventBus;
        }

        public abstract void Attack(TEnemy enemy, Player target);

        protected void CheckDistance(TEnemy enemy, Player target)
        {
            Vector3 toTarget = target.CachedPosition - enemy.CachedPosition;
            var sqrAttackDistance = enemy.AtackDistance * enemy.AtackDistance;

            enemy.InAttackRange = toTarget.sqrMagnitude <= sqrAttackDistance;
        }
    }
}

