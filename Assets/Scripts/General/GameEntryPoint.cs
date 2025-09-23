using DI;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitDI()
    {        
        var installer = FindFirstObjectByType<Installer>(FindObjectsInactive.Include);
        installer.Init();
    }
}
