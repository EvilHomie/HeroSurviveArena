using System;
using UnityEngine;

namespace GameSystem
{
    public class EventSystem : MonoBehaviour
    {
        public Action InitGame;
        public Action StartGame;
        public Action GameOver;
        public Action Victory;

        public Action<Enemy> EnemySpawn;
        public Action<Enemy> EnemyDie;
    }
}

