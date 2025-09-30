using System;
using UnityEngine;

// todo : в финальной версии ко всем Action добавить event. Сейчас сугубо для удобства просмотра ссылок
namespace GameSystem
{
    public class GameFlowSystem : MonoBehaviour
    {        
        [SerializeField] GameState test;
        public Action<GameState> ChangeGameState { get; set; }
        public Action SystemsUpdateTick { get; set; }
        public Action LateUpdateTick { get; set; }
        public GameState СurrentState { get; private set; }

        private bool _requestToChangeState;
        private GameState _newState;

        public void ChangeStateRequest(GameState state)
        {
            _requestToChangeState = true;
            _newState = state;            
        }

        private void ConfirmNewState()
        {
            СurrentState = _newState;
            test = _newState;
            _requestToChangeState = false;
            ChangeGameState?.Invoke(_newState);
        }

        private void Update()
        {
            if (СurrentState != test) // кусок для тестирования через редактор
            {
                СurrentState = test;
                ChangeGameState?.Invoke(test);
            }

            if (СurrentState == GameState.Playing)
            {
                SystemsUpdateTick?.Invoke();
            }

            if (_requestToChangeState)
            {
                ConfirmNewState();
            }
        }

        private void LateUpdate()
        {
            if (СurrentState == GameState.Playing)
            {
                LateUpdateTick?.Invoke();
            }
        }
    }
}