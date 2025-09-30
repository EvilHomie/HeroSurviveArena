using DI;
using System;
using UnityEngine;

namespace GameSystem
{
    public class GameFlowSystem : MonoBehaviour
    {
        [SerializeField] GameState test;
        public Action SystemsUpdateTick { get; set; }
        public Action LateUpdateTick { get; set; }

        private GameEventBus _eventBus;
        private GameState _currentState;

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

        private void Subscribe()
        {
            _eventBus.ChangeGameState += ChangeState;
        }
        private void Unsubscribe()
        {
            _eventBus.ChangeGameState -= ChangeState;
        }

        public void ChangeState(GameState state)
        {
            _currentState = state;
            test = state;
        }

        private void Update()
        {
            if (_currentState != test) // кусок для тестирования через редактор
            {
                _currentState = test;
                _eventBus.ChangeGameState?.Invoke(test);
            }



            if (_currentState == GameState.Playing)
            {
                SystemsUpdateTick?.Invoke();
            }
        }

        private void LateUpdate()
        {
            if (_currentState == GameState.Playing)
            {
                LateUpdateTick?.Invoke();
            }
        }
    }
}