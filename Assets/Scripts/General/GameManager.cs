using DI;
using System;
using UnityEngine;

namespace GameSystem
{
    public class GameManager : MonoBehaviour
    {
       private GameEventBus _eventBus;
       private GameFlowSystem _gameFlowSystem;

        [Inject]
        public void Construct(GameEventBus eventBus, GameFlowSystem gameFlowSystem)
        {
            _eventBus = eventBus;
            _gameFlowSystem = gameFlowSystem;   
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
            _gameFlowSystem.ChangeStateRequest(GameState.Init);
        }

        private void OnPlayerDie(Player player)
        {
            _gameFlowSystem.ChangeStateRequest(GameState.GameOver);
        }
    }
}

