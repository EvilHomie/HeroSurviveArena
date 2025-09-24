using GameSystem;

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
                enemy.IsDead = true;
                _eventBus.EnemyDie?.Invoke(enemy);
            }
        }
    }
}