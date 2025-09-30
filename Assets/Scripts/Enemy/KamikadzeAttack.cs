using GameSystem;

namespace Enemy
{
    public class KamikadzeAttack : EnemyAttackBase<Kamikadze>
    {
        public KamikadzeAttack(GameEventBus gameEventBus) : base(gameEventBus)
        {
        }

        public override void Attack(Kamikadze enemy, Player target)
        {
            CheckDistance(enemy, target);

            if (enemy.InAttackRange)
            {
                target.CurrentHealthPoint -= enemy.AttackDamage;
                EventBus.EnemyDie?.Invoke(enemy);
            }
        }
    }
}