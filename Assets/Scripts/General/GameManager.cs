using DI;
using System;
using UnityEngine;

namespace GameSystem
{
    public class GameManager : MonoBehaviour
    {
        GameEventBus _eventBus;

        [Inject]
        public void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void OnEnable()
        {
            _eventBus.PlayerDie += OnPlayerDie;
        }

        private void OnDisable()
        {
            _eventBus.PlayerDie -= OnPlayerDie;
        }

        private void Start()
        {
            _eventBus.ChangeGameState?.Invoke(GameState.Init);
            
        }

        private void OnPlayerDie(Player player)
        {
            _eventBus.ChangeGameState?.Invoke(GameState.GameOver);
        }
    }
}

