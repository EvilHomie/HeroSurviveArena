using UnityEngine;

namespace DI
{
    public class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitGame()
        {
            InitDI();
        }

        private static void InitDI()
        {
            var projectInstallerPF = Resources.Load<ProjectInstaller>("ProjectInstaller");
            var projectInstaller = Object.Instantiate(projectInstallerPF);
            Object.DontDestroyOnLoad(projectInstaller.gameObject);
            projectInstaller.Init();
        }
    }
}
