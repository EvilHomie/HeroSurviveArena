using GameSystem;
using System;
using UnityEngine;

namespace Projectile
{
    public abstract class MovementBehaviorBase<TProjectile> : IMovementBehavior<TProjectile> where TProjectile : ProjectileBase
    {
        private readonly Action<TProjectile> _moveAction;
        private readonly GameEventBus _eventBus;

        public MovementBehaviorBase(GameEventBus eventBus)
        {
            _eventBus = eventBus;
            _moveAction = Move;
        }       

        public virtual void Move(TProjectile projectile)
        {
            projectile.CachedTransform.position += projectile.Velocity * Time.deltaTime;
            projectile.CachedPosition = projectile.CachedTransform.position;
            CheckLifeTime(projectile);
        }

        private void CheckLifeTime(TProjectile projectile)
        {
            projectile.LeftLifeTime -= Time.deltaTime;

            if (projectile.LeftLifeTime <= 0)
            {
                _eventBus.ProjectileDie(projectile);
            }
        }

        void IMovementBehaviorBase.Move(ProjectileBase projectile)
        {
            _moveAction((TProjectile)projectile);
        }
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