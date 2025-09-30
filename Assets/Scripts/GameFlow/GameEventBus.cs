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
        public Action<Player> PlayerDie { get; set; }
        public Action<Player> PlayerShoot { get; set; }
        public Action<Shooter> EnemyShoot { get; set; }
        public Action<ProjectileBase> ProjectileDie { get; set; }
    }
}

