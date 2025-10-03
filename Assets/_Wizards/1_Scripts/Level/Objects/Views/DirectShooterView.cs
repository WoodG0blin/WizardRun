using UnityEngine;

namespace WizardsPlatformer
{
    internal class DirectShooterView : LevelObjectView
    {
        [field: SerializeField] public Transform Barrel { get; private set; }
        [field: SerializeField] public Transform BarrelRadius { get; private set; }

        public void Init()
        {
            Barrel ??= transform;
        }

        public Vector2 RotateBarrelTowards(Vector3 targetPosition)
        {
            BarrelRadius.right = (targetPosition - Position).normalized;
            return new(BarrelRadius.right.x, BarrelRadius.right.y);
        }
    }
}