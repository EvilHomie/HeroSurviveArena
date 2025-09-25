using GameSystem;
using UnityEngine;

namespace Enemy
{
    public class RangedAttackBehavior : AbstractAttackBehavior<EnemyShooter>
    {
        
        public RangedAttackBehavior(GameEventBus gameEventBus) : base(gameEventBus)
        {
        }

        public override void Attack(EnemyShooter enemy, Player target)
        {
            Reload(enemy);

            if (enemy.InAttackRange)
            {
                ProcessShooting(enemy);
            }
        }

        private void Reload(EnemyShooter enemy)
        {
            if (!enemy.IsReloaded)
            {
                enemy.ReloadTimer -= Time.deltaTime;
                enemy.IsReloaded = enemy.ReloadTimer <= 0;
            }
        }

        private void ProcessShooting(EnemyShooter enemy)
        {
            if (enemy.IsReloaded)
            {
                Shoot(enemy);
                enemy.IsReloaded = false;
                enemy.ReloadTimer = 1 / enemy.AttackRate;
            }
        }

        private void Shoot(EnemyShooter enemy)
        {
            _eventBus.EnemyShoot(enemy);
        }
    }
}