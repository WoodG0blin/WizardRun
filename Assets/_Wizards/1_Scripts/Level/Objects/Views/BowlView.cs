using UnityEngine;

namespace WizardsPlatformer
{
    internal class BowlView : LevelObjectView
    {
        [field: SerializeField] public Transform Barrel { get; private set; }
        [field: SerializeField] public Transform BarrelRadius { get; private set; }

        public void Init()
        {
            Barrel ??= transform;
        }

        public Vector2 RotateBarrelTowards(Vector3 targetPosition)
        {
            BarrelRadius.forward = (targetPosition - Position).normalized;
            return new(BarrelRadius.forward.x, BarrelRadius.forward.y);
        }
    }
}