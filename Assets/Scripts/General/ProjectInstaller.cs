using GameInput;
using GameSystem;
using UnityEngine;

namespace DI
{
    public class ProjectInstaller : Installer
    {
        [SerializeField] GameFlowSystem _gameFlowSystem;
        [SerializeField] GameEventBus _eventBus;
        [SerializeField] Player _player;
        [SerializeField] Config _config;
        [SerializeField] Camera _camera;
        [SerializeField] EnemiesPool _enemiesPool;
        [SerializeField] ProjectilePool _projectilePool;
        protected override void InstallBindings()
        {
            Container.Bind<GameFlowSystem>().FromInstance(_gameFlowSystem);
            Container.Bind<Player>().FromInstance(_player);
            Container.Bind<Config>().FromInstance(_config);
            Container.Bind<Camera>().FromInstance(_camera);
            Container.Bind<GameEventBus>().FromInstance(_eventBus);
            Container.Bind<EnemiesPool>().FromInstance(_enemiesPool);
            Container.Bind<ProjectilePool>().FromInstance(_projectilePool);

            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.isEditor)
            {
                Container.Bind<IPlayerInput>().To<PCInput>().AsSingleton();
            }
        }
    }
}


