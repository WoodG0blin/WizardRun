using System;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace WizardsPlatformer
{
    internal class FlyingEnemyView : PatrollingEnemyView
    {
        [SerializeField] private string targetAccessable;

        public override bool TryMoveTo(Vector2 target, float speed)
        {
            if (CheckApproach(target, Position)) return false;
            if (CheckStep(target - Position, Position))
            {
                Mover?.SetInput(target - Position, speed);
                return true;
            }
            return false;
        }


        public override bool IsPointAccessable(Vector2 targetPoint) => true;

        protected override bool CheckApproach(Vector2 point, Vector2 origin)
        {
            return (point - origin).magnitude < 0.15f;
        }

        protected override bool CheckStep(Vector2 direction, Vector2 origin)
        {
            hasObstacle = Physics.Raycast(origin, direction.normalized, 0.5f);
            CheckStepText = $"{direction}. Has obstacle: {hasObstacle}";
            Debug.DrawRay(origin, direction.normalized, Color.green, 0.5f);
            return !hasObstacle;
        }
    }
}