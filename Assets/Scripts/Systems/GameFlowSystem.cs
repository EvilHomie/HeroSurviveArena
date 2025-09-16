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
    MainMenu,       // Главное меню
    GameMenu,       // Меню в игре
    Loading,        // Загрузка сцены или ресурсов
    Playing,        // Игровой процесс
    Paused,         // Пауза
    GameOver,       // Конец игры
    Victory,        // Победа
    Cutscene,       // Видеоролик или сюжетное событие (анимация)
}