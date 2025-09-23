using DI;
using GameSystem;
using UnityEngine;

namespace Enemy
{
    public class Ranged : AbstractEnemy
    {
        [field: SerializeField] public bool IsMaintainingDistance { get; private set; }
        [field: SerializeField] public bool CanAttackWhileMoving { get; private set; }

        [SerializeField] float _atackRate;
        private GameFlowSystem _gameFlowSystem;
        private bool _isAttacking;

        [Inject]
        public void Construct(GameFlowSystem gameFlowSystem)
        {
            _gameFlowSystem = gameFlowSystem;
            _gameFlowSystem.UpdateTick += OnUpdateTick;
        }

        public override void ResetData()
        {
            base.ResetData();
            _isAttacking = false;
        }

        public override void Atack()
        {            
            _isAttacking = true;
        }
        
        public void StopAAttack()
        {
            _isAttacking = false;
        }

        private void OnUpdateTick()
        {
            if (_isAttacking)
            {
                Debug.Log("ATACKING");
            }
        }
    }
}