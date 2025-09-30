using System;
using UnityEngine;

// todo : � ��������� ������ �� ���� Action �������� event. ������ ������ ��� �������� ��������� ������
namespace GameSystem
{
    public class GameFlowSystem : MonoBehaviour
    {        
        [SerializeField] GameState test;
        public Action<GameState> ChangeGameState { get; set; }
        public Action SystemsUpdateTick { get; set; }
        public Action LateUpdateTick { get; set; }
        public GameState �urrentState { get; private set; }

        private bool _requestToChangeState;
        private GameState _newState;

        public void ChangeStateRequest(GameState state)
        {
            _requestToChangeState = true;
            _newState = state;            
        }

        private void ConfirmNewState()
        {
            �urrentState = _newState;
            test = _newState;
            _requestToChangeState = false;
            ChangeGameState?.Invoke(_newState);
        }

        private void Update()
        {
            if (�urrentState != test) // ����� ��� ������������ ����� ��������
            {
                �urrentState = test;
                ChangeGameState?.Invoke(test);
            }

            if (�urrentState == GameState.Playing)
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
            if (�urrentState == GameState.Playing)
            {
                LateUpdateTick?.Invoke();
            }
        }
    }
}