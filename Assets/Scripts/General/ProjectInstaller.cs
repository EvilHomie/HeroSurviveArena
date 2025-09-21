using GameInput;
using GameSystem;
using UnityEngine;

namespace DI
{
    public class ProjectInstaller : Installer
    {
        [SerializeField] GameFlowSystem _gameFlowSystem;
        [SerializeField] PlayerContainer _playerContainer;
        [SerializeField] Config _config;
        [SerializeField] Camera _camera;
        [SerializeField] GameEventBus _eventBus;
        [SerializeField] EnemiesPool _enemiesPool;
        protected override void InstallBindings()
        {
            _container.Bind<GameFlowSystem>().FromInstance(_gameFlowSystem);
            _container.Bind<PlayerContainer>().FromInstance(_playerContainer);
            _container.Bind<Config>().FromInstance(_config);
            _container.Bind<Camera>().FromInstance(_camera);
            _container.Bind<GameEventBus>().FromInstance(_eventBus);
            _container.Bind<EnemiesPool>().FromInstance(_enemiesPool);

            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.isEditor)
            {
                _container.Bind<IPlayerInput>().To<PCInput>().AsSingleton();
            }
        }
    }
}


