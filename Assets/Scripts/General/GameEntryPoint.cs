using DI;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    GameFlowSystem _gameFlowSystem;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitGame()
    {
        InitDI();
    }
    private static void InitDI()
    {
        var installer = FindFirstObjectByType<Installer>(FindObjectsInactive.Include);
        installer.Init();
    }

    [Inject]
    public void Constructor(GameFlowSystem gameFlowSystem)
    {
        _gameFlowSystem = gameFlowSystem;
    }

    private void Start()
    {
        _gameFlowSystem.StartGame();
        _gameFlowSystem.ChangeFlowState(GameFlowState.Playing);
    }
}
