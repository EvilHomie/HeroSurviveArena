using Enemy;
using Projectile;
using System;
using UnityEngine;

namespace GameSystem
{
    public class GameEventBus : MonoBehaviour
    {
        public Action<GameState> ChangeGameState { get; set; }
        public Action<EnemyBase> EnemySpawn { get; set; }
        public Action<EnemyBase> EnemyDie { get; set; }


        public Action<Player> PlayerShoot;
        public Action<EnemyShooter> EnemyShoot;
        public Action<ProjectileBase> ProjectileDie { get; set; }
    }
}

