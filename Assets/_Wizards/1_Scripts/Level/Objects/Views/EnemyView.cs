using TMPro;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class EnemyView : LevelObjectView
    {
        [field: SerializeField] public Transform Barrel { get; private set; }
        [field: SerializeField] public Transform BarrelRadius { get; private set; }
        [SerializeField] private MovementType _movementType;

        protected override void OnInitiation()
        {
            Barrel ??= transform;
            SetMover(_movementType);
        }

        public override void SetTargetDirection(Vector3 direction)
        {
            base.SetTargetDirection(direction);
            BarrelRadius.right = (direction - Position).normalized;
        }
    }
}