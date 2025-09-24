using DI;
using System;
using UnityEngine;

namespace GameSystem
{
    public class GameFlowSystem : MonoBehaviour
    {
        [SerializeField] GameFlowState test;
        public Action PreUpdateTick { get; set; }
        public Action UpdateTick { get; set; }
        public Action LateUpdateTick { get; set; }
        public Action<GameFlowState> ChangeGameState { get; set; }

        private GameEventBus _eventBus;
        private GameFlowState _state;

        [Inject]
        public void Construct(GameEventBus eventBus)
        {
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

        public void ChangeState(GameFlowState state)
        {
            _state = state;
            ChangeGameState?.Invoke(state);
        }

        private void Update()
        {
            if (_state != test)
            {
                _state = test;
                ChangeState(_state);
            }



            if (_state == GameFlowState.Playing)
            {
                PreUpdateTick?.Invoke();
                UpdateTick?.Invoke();
            }




        }

        private void LateUpdate()
        {
            LateUpdateTick?.Invoke();
        }

        private void OnGameOver()
        {
            ChangeState(GameFlowState.GameOver);
        }
        private void OnVictory()
        {
            ChangeState(GameFlowState.Victory);
        }

        private void Subscribe()
        {
            _eventBus.GameOver += OnGameOver;
            _eventBus.Victory += OnVictory;
        }
        private void Unsubscribe()
        {
            _eventBus.GameOver -= OnGameOver;
            _eventBus.Victory -= OnVictory;
        }
    }
}