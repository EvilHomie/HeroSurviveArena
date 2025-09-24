using Enemy;
using System;
using UnityEngine;

namespace GameSystem
{
    public class GameEventBus : MonoBehaviour
    {
        public Action<GameState> ChangeGameState { get; set; }
        public Action<AbstractEnemy> EnemySpawn { get; set; }
        public Action<AbstractEnemy> EnemyDie { get; set; }
    }
}

