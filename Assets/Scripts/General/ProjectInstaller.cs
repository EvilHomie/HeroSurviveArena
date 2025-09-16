using DI;
using UnityEngine;

public class ProjectInstaller : Installer
{
    [SerializeField] GameFlowSystem _gameFlowSystem;
    [SerializeField] PlayerContainer _playerContainer;
    [SerializeField] Config _config;
    [SerializeField] Camera _camera;
    protected override void InstallBindings()
    {
        _container.Bind<GameFlowSystem>().FromInstance(_gameFlowSystem);
        _container.Bind<PlayerContainer>().FromInstance(_playerContainer);
        _container.Bind<Config>().FromInstance(_config);
        _container.Bind<Camera>().FromInstance(_camera);

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.isEditor)
        {
            _container.Bind<IPlayerInput>().To<PCInput>().AsSingleton();
        }
    }
}
