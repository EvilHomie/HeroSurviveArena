using GameSystem;

namespace Projectile
{
    public class ProjectileHomingBehavior : MovementBehaviorBase<ProjectileHoming>
    {
        public ProjectileHomingBehavior(GameEventBus eventBus) : base(eventBus)
        {
        }

        public override void Move(ProjectileHoming projectile)
        {
            var newDirection = projectile.TargetTransform.position - projectile.CachedPosition;
            projectile.Velocity = newDirection.normalized * projectile.Speed;

            if (projectile.AlignToDirection)
            {
                projectile.CachedTransform.forward = newDirection;
            }

            base.Move(projectile);
        }
    }
}