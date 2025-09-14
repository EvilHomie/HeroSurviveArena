using UnityEngine;

namespace DI
{
    public class ProjectInstaller : MonoBehaviour
    {
        private Container _container;

        public void Init()
        {
            _container = new();
            InstallBindings();
            InjectSceneMonobehaviours();
        }

        private void InjectSceneMonobehaviours()
        {
            var sceneMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var monoBehaviour in sceneMonoBehaviours)
            {
                _container.InjectMonoBehaviour(monoBehaviour);
            }
        }

        private void InstallBindings()
        {
           
        }
    }
}
