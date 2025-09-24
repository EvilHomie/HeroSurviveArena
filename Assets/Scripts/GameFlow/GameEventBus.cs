using Enemy;
using System;
using UnityEngine;

namespace GameSystem
{
    public class GameEventBus : MonoBehaviour
    {
        public Action InitGame { get; set; }
        public Action StartGame { get; set; }
        public Action GameOver { get; set; }
        public Action Victory { get; set; }

        public Action<AbstractEnemy> EnemySpawn { get; set; }
        public Action<AbstractEnemy> EnemyDie { get; set; }
    }
}

