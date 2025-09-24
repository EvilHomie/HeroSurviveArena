using GameSystem;
using UnityEngine;

namespace Enemy
{
    public class KamikadzeAttackBehavior : AbstractAttackBehavior<Kamikadze>
    {
        public KamikadzeAttackBehavior(GameEventBus gameEventBus) : base(gameEventBus)
        {
        }

        public override void Attack(Kamikadze enemy, Player target)
        {
            if (enemy.InAttackRange)
            {
                target.CurrentHealthPoint -= enemy.AttackDamage;

                if (target.CurrentHealthPoint <= 0)
                {
                    _eventBus.GameOver?.Invoke();
                }

                enemy.IsDead = true;
                _eventBus.EnemyDie?.Invoke(enemy);
            }
        }
    }
}