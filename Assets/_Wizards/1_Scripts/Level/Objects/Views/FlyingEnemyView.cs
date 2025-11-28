using System;
using TMPro;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class FlyingEnemyView : PatrollingEnemyView
    {
        [SerializeField] private string targetAccessable;

        public override bool TryMoveTo(Vector2 target, float speed)
        {
            if (CheckApproach(target, Position)) return false;
            if (CheckStep(target, Position))
            {
                Mover?.SetInput(target - Position, speed);
                return true;
            }
            return false;
        }


        public override bool IsPointAccessable(Vector2 targetPoint) => true;

        protected override bool CheckApproach(Vector2 point, Vector2 origin)
        {
            return (point - origin).magnitude < 0.1f;
        }
    }
}