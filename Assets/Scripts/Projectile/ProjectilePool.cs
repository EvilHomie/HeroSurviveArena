using Projectile;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class ProjectilePool : AbstractPool<ProjectileBase>
    {
        private GameObject _projectileContainer;
        private readonly List<string> _projectileNames = new();

        private void Awake()
        {
            _projectileContainer = new GameObject("Pool_Container_Projectile");

            foreach (ProjectileBase projectile in _config.Projectiles)
            {
                if (projectile == null) continue;

                _projectileNames.Add(projectile.UsedName);
                CreateItemPool(projectile, _config.ProjectilePoolStartCapacity, _config.ProjectilePoolMaxCapacity, _projectileContainer.transform, _config.ProjectilePoolPrewarmCount);
            }
        }

        protected override void Subscribe()
        {
            _gameEventBus.ProjectileDie += OnItemDeactivated;
            _gameEventBus.ChangeGameState += OnChangeGameState;
            _gameFlowSystem.UpdateTick += ReleaseInactive;
        }

        protected override void Unsubscribe()
        {
            _gameEventBus.ProjectileDie -= OnItemDeactivated;
            _gameEventBus.ChangeGameState -= OnChangeGameState;
            _gameFlowSystem.UpdateTick -= ReleaseInactive;
        }

        private void OnChangeGameState(GameState gameState)
        {
            if (gameState == GameState.GameOver || gameState == GameState.Victory)
            {
                ReleaseAll();
            }
        }
    }
}
