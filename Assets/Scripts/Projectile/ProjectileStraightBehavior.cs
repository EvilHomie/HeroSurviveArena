using GameSystem;

namespace Projectile
{
    public class ProjectileStraightBehavior : MovementBehaviorBase<ProjectileStraight>
    {
        public ProjectileStraightBehavior(GameEventBus eventBus) : base(eventBus)
        {
        }
        // можно добавить логику переписав виртуальный метод Move
    }
}
