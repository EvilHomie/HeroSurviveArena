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

            foreach (ProjectileBase projectile in Config.Projectiles)
            {
                if (projectile == null) continue;

                _projectileNames.Add(projectile.UsedName);
                CreateItemPool(projectile, Config.ProjectilePoolStartCapacity, Config.ProjectilePoolMaxCapacity, _projectileContainer.transform, Config.ProjectilePoolPrewarmCount);
            }
        }

        protected override void Subscribe()
        {
            GameEventBus.ProjectileDie += OnItemDeactivated;
            GameEventBus.ChangeGameState += OnChangeGameState;
            GameFlowSystem.UpdateTick += ReleaseInactive;
        }

        protected override void Unsubscribe()
        {
            GameEventBus.ProjectileDie -= OnItemDeactivated;
            GameEventBus.ChangeGameState -= OnChangeGameState;
            GameFlowSystem.UpdateTick -= ReleaseInactive;
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
