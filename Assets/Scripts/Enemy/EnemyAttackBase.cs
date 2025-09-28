using GameSystem;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyAttackBase<TEnemy> where TEnemy : EnemyBase
    {
        protected GameEventBus _eventBus;

        public EnemyAttackBase(GameEventBus gameEventBus)
        {
            _eventBus = gameEventBus;
        }

        public virtual void Attack(TEnemy enemy, Player target)
        {
            CheckDistance(enemy, target);
        }

        private void CheckDistance(TEnemy enemy, Player target)
        {
            Vector3 toTarget = target.CachedPosition - enemy.CachedPosition;
            var sqrAttackDistance = enemy.AtackDistance * enemy.AtackDistance;

            enemy.InAttackRange = toTarget.sqrMagnitude <= sqrAttackDistance;
        }
    }
}

