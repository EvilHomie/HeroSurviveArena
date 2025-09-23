using Enemy;
using System;
using UnityEngine;

namespace GameSystem
{
    public class GameEventBus : MonoBehaviour
    {
        public Action InitGame;
        public Action StartGame;
        public Action GameOver;
        public Action Victory;

        public Action<AbstractEnemy> EnemySpawn;
        public Action<AbstractEnemy> EnemyDie;
    }
}

