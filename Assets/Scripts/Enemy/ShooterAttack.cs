using GameSystem;
using UnityEngine;

namespace Enemy
{
    public class ShooterAttack : EnemyAttackBase<Shooter>
    {
        
        public ShooterAttack(GameEventBus gameEventBus) : base(gameEventBus)
        {
        }

        public override void Attack(Shooter enemy, Player target)
        {
            base.Attack(enemy, target);
            Reload(enemy);

            if (enemy.InAttackRange)
            {
                ProcessShooting(enemy);
            }
        }

        private void Reload(Shooter enemy)
        {
            if (!enemy.IsReloaded)
            {
                enemy.ReloadTimer -= Time.deltaTime;
                enemy.IsReloaded = enemy.ReloadTimer <= 0;
            }
        }

        private void ProcessShooting(Shooter enemy)
        {
            if (enemy.IsReloaded)
            {
                Shoot(enemy);
                enemy.IsReloaded = false;
                enemy.ReloadTimer = 1 / enemy.AttackRate;
            }
        }

        private void Shoot(Shooter enemy)
        {
            _eventBus.EnemyShoot(enemy);
        }
    }
}