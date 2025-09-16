using System;
using UnityEngine;

public class GameFlowSystem : MonoBehaviour
{
    public event Action CustomUpdate;
    public event Action CustomLateUpdate;
    public event Action<GameFlowState> ChangeGameState;
    public event Action CustomStart;
    public event Action OnAppQuit;

    public void StartGame()
    {
        CustomStart?.Invoke();
    }

    public void ChangeFlowState(GameFlowState flow)
    {
        ChangeGameState?.Invoke(flow);
    }

    private void Update()
    {
        CustomUpdate?.Invoke();
    }

    private void LateUpdate()
    {
        CustomLateUpdate?.Invoke();
    }

    private void OnApplicationQuit()
    {
        OnAppQuit?.Invoke();
    }
}

public enum GameFlowState
{
    MainMenu,       // ������� ����
    GameMenu,       // ���� � ����
    Loading,        // �������� ����� ��� ��������
    Playing,        // ������� �������
    Paused,         // �����
    GameOver,       // ����� ����
    Victory,        // ������
    Cutscene,       // ���������� ��� �������� ������� (��������)
}