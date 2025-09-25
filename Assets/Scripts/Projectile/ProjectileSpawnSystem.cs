using DI;
using Enemy;
using System;
using UnityEngine;

namespace GameSystem
{
    public class ProjectileSpawnSystem : MonoBehaviour
    {
        private ProjectilePool _projectilePool;
        private Player _player;
        private GameEventBus _eventBus;

        [Inject]
        public void Construct(ProjectilePool projectilePool, Player player, GameEventBus eventBus)
        {
            _projectilePool = projectilePool;
            _player = player;
            _eventBus = eventBus;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _eventBus.PlayerShoot += OnPlayerShoot;
            _eventBus.EnemyShoot += OnEnemyShoot;
        }

        private void Unsubscribe()
        {
            _eventBus.PlayerShoot -= OnPlayerShoot;
            _eventBus.EnemyShoot -= OnEnemyShoot;
        }

        private void OnEnemyShoot(EnemyShooter enemy)
        {
            var instance = _projectilePool.Getitem(enemy.ShootProjectile.UsedName);
            instance.Config(_player.transform, enemy.ProjectileSpeed, enemy.CashedType, enemy.AttackDamage, enemy.CachedPosition);
            instance.CachedGameObject.SetActive(true);
            Debug.Log(instance.UsedName);
        }

        private void OnPlayerShoot(Player player)
        {
            throw new NotImplementedException();
        }
    }
}