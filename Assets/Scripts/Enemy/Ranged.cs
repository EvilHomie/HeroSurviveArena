using UnityEngine;

namespace Enemy
{
    public class Ranged : AbstractEnemy
    {
        [field: SerializeField] public float MaintainingDistance { get; private set; }
        [field: SerializeField] public bool CanAttackWhileMoving { get; private set; }
        [field: SerializeField] public float AttackRate { get; private set; }
        public float ReloadTimer { get; set; }
        public bool IsReloaded {  get; set; }
    }
}