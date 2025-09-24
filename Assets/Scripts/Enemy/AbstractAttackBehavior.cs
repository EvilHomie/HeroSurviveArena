using GameSystem;
using UnityEngine;

namespace Enemy
{
    public abstract class AbstractAttackBehavior<TEnemy> : IAttackBehavior<TEnemy> where TEnemy : AbstractEnemy
    {
        protected GameEventBus _eventBus;

        public AbstractAttackBehavior(GameEventBus gameEventBus)
        {
            _eventBus = gameEventBus;
        }

        public abstract void Attack(TEnemy enemy, Player target);

        public void CheckDistance(TEnemy enemy, Player target)
        {
            Vector3 toTarget = target.CachedPosition - enemy.CachedPosition;
            var sqrAttackDistance = enemy.AtackDistance * enemy.AtackDistance;

            enemy.InAttackRange = toTarget.sqrMagnitude <= sqrAttackDistance;
        }

        void IAttackBehaviorBase.Attack(AbstractEnemy enemy, Player target)
                => Attack((TEnemy)enemy, target);
        void IAttackBehaviorBase.CheckDistance(AbstractEnemy enemy, Player target)
                => CheckDistance((TEnemy)enemy, target);
    }

    public interface IAttackBehaviorBase
    {
        void Attack(AbstractEnemy enemy, Player target);
        void CheckDistance(AbstractEnemy enemy, Player target);
    }

    public interface IAttackBehavior<TEnemy> : IAttackBehaviorBase where TEnemy : AbstractEnemy
    {
        void Attack(TEnemy enemy, Player target);
        void CheckDistance(TEnemy enemy, Player target);
    }
}

