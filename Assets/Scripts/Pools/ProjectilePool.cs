using Projectile;
using UnityEngine;

namespace GameSystem
{
    public class ProjectilePool : AbstractPool<ProjectileBase>
    {
        private GameObject _projectileContainer;

        private void Awake()
        {
            _projectileContainer = new GameObject("Pool_Container_Projectile");
            CreateItemPools(Config.Projectiles, Config.ProjectilePoolStartCapacity, Config.ProjectilePoolMaxCapacity, _projectileContainer.transform, Config.ProjectilePoolPrewarmCount);
        }

        protected override void Subscribe()
        {
            EventBus.ProjectileDie += ScheduleForRelease;
            GameFlowSystem.ChangeGameState += OnChangeGameState;
            GameFlowSystem.SystemsUpdateTick += ReleasePendingItems;
        }

        protected override void Unsubscribe()
        {
            EventBus.ProjectileDie -= ScheduleForRelease;
            GameFlowSystem.ChangeGameState -= OnChangeGameState;
            GameFlowSystem.SystemsUpdateTick -= ReleasePendingItems;
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
