using GameSystem;

namespace Projectile
{
    public class ProjectileStraightBehavior : MovementBehaviorBase<ProjectileStraight>
    {
        public ProjectileStraightBehavior(GameEventBus eventBus) : base(eventBus)
        {
        }
        // ����� �������� ������ ��������� ����������� ����� Move
    }
}
