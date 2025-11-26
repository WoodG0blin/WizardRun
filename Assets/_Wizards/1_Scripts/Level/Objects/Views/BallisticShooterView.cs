using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardsPlatformer
{
    internal class BallisticShooterView : LevelObjectView
    {
        [field: SerializeField] public Transform Barrel { get; private set; }

        protected override void OnInitiation()
        {
            Barrel ??= transform;
        }

        public Vector2 RotateBarrelTowards(Vector2 targetPosition)
        {
            return targetPosition - Position;
        }
    }
}