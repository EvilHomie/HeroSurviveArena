using GameSystem;
using UnityEngine;

namespace Enemy
{
    public class RangedAttackBehavior : AbstractAttackBehavior<Ranged>
    {
        
        public RangedAttackBehavior(GameEventBus gameEventBus) : base(gameEventBus)
        {
        }

        public override void Attack(Ranged enemy, Player target)
        {
            Reload(enemy);

            if (enemy.InAttackRange)
            {
                ProcessShooting(enemy);
            }
        }

        private void Reload(Ranged enemy)
        {
            if (!enemy.IsReloaded)
            {
                enemy.ReloadTimer -= Time.deltaTime;
                enemy.IsReloaded = enemy.ReloadTimer <= 0;
            }
        }

        private void ProcessShooting(Ranged enemy)
        {
            if (enemy.IsReloaded)
            {
                Shoot(enemy);
                enemy.IsReloaded = false;
                enemy.ReloadTimer = 1 / enemy.AttackRate;
            }
        }

        private void Shoot(Ranged enemy)
        {
            Debug.Log($"{enemy.name} OneShoot");
        }
    }
}