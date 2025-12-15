using System;
using UnityEngine;
using TMPro;

namespace WizardsPlatformer
{
    internal class PatrollingEnemyView : EnemyView
    {
        public string StateText;
        public string CheckStepText;
        public bool hasObstacle;
        public bool hasGap;
        [field: SerializeField] public Vector2 PatrolOffset { get; protected set; }
        
        protected LayerMask _groundsLayerMask;
        [SerializeField] protected Vector2 _patrolPoint2;
        [SerializeField] protected Vector2 _patrolPoint1;
        [SerializeField] protected Vector2 _position;

        [field: SerializeField] public Vector2 NextPatrolPoint { get; protected set; }

        protected override sealed void OnInitiation()
        {
            base.OnInitiation();

            _patrolPoint1 = Position + PatrolOffset;
            _patrolPoint2 = Position - PatrolOffset;
            NextPatrolPoint = _patrolPoint1;

            _groundsLayerMask = LayerMask.GetMask("Background");
        }

        protected override sealed void OnUpdate()
        {
            //animator.UpdateValues(rigidbody.velocity);
            _position = Position;
        }

        public virtual void SwitchPatrolPoint() => NextPatrolPoint = NextPatrolPoint == _patrolPoint1 ? _patrolPoint2 : _patrolPoint1;

        public virtual bool TryMoveTo(Vector2 targetPoint, float speed)
        {
            CheckStepText = $"to {targetPoint}. {CheckStep(targetPoint, Position)}";
            if (CheckApproach(targetPoint, Position) || !CheckStep(targetPoint - Position, Position)) return false;
            else
            {
                Mover?.SetInput(targetPoint, speed);
                return true;
            }
        }

        protected virtual bool CheckApproach(Vector2 point, Vector2 origin)
        {
            return Mathf.Abs(point.x - origin.x) < 0.05f;
        }

        protected virtual bool CheckStep(Vector2 direction, Vector2 origin)
        {
            bool res = true;
            Vector2 directionNormalized = new(direction.x > 0 ? 1 : -1, 0);

            hasObstacle = Physics.Raycast(origin, directionNormalized, 0.5f);
            hasGap = !Physics.Raycast(origin + directionNormalized, Vector3.down, 1.5f, _groundsLayerMask);

            if (hasObstacle || hasGap) res = false;
            return res;
        }

        public virtual bool IsPointAccessable(Vector2 point)
        {
            float sensitivity = 0.5f;
            Vector2 checkPos = Position;
            Vector2 direction = point - checkPos;
            Vector2 step = direction.normalized * sensitivity;
            int steps = Mathf.RoundToInt(direction.magnitude / sensitivity);

            bool res = true;
            for (int i = 1; i < steps; i++)
            {
                if (!CheckStep(checkPos + step, checkPos))
                {
                    res = false;
                    break;
                }
                checkPos += step;
            }

            return res;
        }

        //public override sealed void SetLookDirection(Vector2 direction)
        //{
        //    XDirection = visualBody.right.x;
        //}

        public void DisplayAttack(Action onAttackPositionReady) =>
            animator.TriggerAnimation(ActionState.Attack, () => onAttackPositionReady());

    }
}