using System;
using UnityEngine;

namespace GameSystem
{
    public class GameFlowSystem : MonoBehaviour
    {
        [SerializeField] GameFlowState test;
        public event Action UpdateTick;
        public event Action LateUpdateTick;
        public event Action<GameFlowState> ChangeGameState;

        private GameFlowState _state;

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
                UpdateTick?.Invoke();
            }



            
        }

        private void LateUpdate()
        {
            LateUpdateTick?.Invoke();
        }
    }
}