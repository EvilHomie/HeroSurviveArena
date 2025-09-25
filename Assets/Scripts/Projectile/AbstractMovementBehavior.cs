using UnityEngine;

namespace Projectile
{
    public abstract class AbstractMovementBehavior<TProjectile> : IMovementBehavior<TProjectile> where TProjectile : ProjectileBase
    {
        public virtual void Move(TProjectile projectile)
        {
            projectile.CachedTransform.position += projectile.Velocity * Time.deltaTime;
            projectile.CachedPosition = projectile.CachedTransform.position;
            Debug.Log("Move");
        }

        void IMovementBehaviorBase.Move(ProjectileBase projectile)
            => Move((TProjectile)projectile);
    }

    public interface IMovementBehaviorBase
    {
        void Move(ProjectileBase projectile);
    }

    public interface IMovementBehavior<TProjectile> : IMovementBehaviorBase
    {
        void Move(TProjectile projectile);
    }
}


//public abstract class AbstractMovementBehavior<TProjectile> : IMovementBehavior<TProjectile> where TProjectile : ProjectileBase
//{
//    // Делегат для вызова Move, настроен один раз
//    private Action<TProjectile> _moveAction;

//    // Инициализация поведения для конкретного снаряда
//    public void Init(ProjectileBase projectile)
//    {
//        if (projectile is not TProjectile typed)
//        {
//            UnityEngine.Debug.LogError(
//                $"[MovementBehavior] Invalid projectile type {projectile.GetType().Name}, expected {typeof(TProjectile).Name}");
//            _moveAction = _ => { }; // пустышка
//        }
//        else
//        {
//            _moveAction = Move; // безопасный вызов без кастов
//        }
//    }

//    // Основной метод движения, который вызывается через делегат
//    public virtual void Move(TProjectile projectile)
//    {
//        projectile.CachedTransform.position += projectile.Velocity * UnityEngine.Time.deltaTime;
//        projectile.CachedPosition = projectile.CachedTransform.position;
//    }

//    // Реализация интерфейса IMovementBehaviorBase
//    void IMovementBehaviorBase.Move(ProjectileBase projectile)
//    {
//        if (_moveAction == null)
//        {
//            UnityEngine.Debug.LogError("[MovementBehavior] Behavior not initialized for this projectile");
//            return;
//        }

//        // Без кастов: делегат уже знает конкретный тип
//        if (projectile is TProjectile typed)
//        {
//            _moveAction(typed);
//        }
//        else
//        {
//            UnityEngine.Debug.LogError(
//                $"[MovementBehavior] Invalid projectile type at runtime: {projectile.GetType().Name}");
//        }
//    }
//}

//// Generic интерфейс для чистого вызова
//public interface IMovementBehavior<TProjectile> : IMovementBehaviorBase
//    where TProjectile : ProjectileBase
//{
//    void Move(TProjectile projectile);
//}

//// Базовый интерфейс для работы с ProjectileBase
//public interface IMovementBehaviorBase
//{
//    void Move(ProjectileBase projectile);
//}