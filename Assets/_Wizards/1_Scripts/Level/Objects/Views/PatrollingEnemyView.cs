using System;
using UnityEngine;
using TMPro;

namespace WizardsPlatformer
{
    internal class PatrollingEnemyView : EnemyView
    {
        public string StateText;
        public string CheckStepText;
        [field: SerializeField] public Vector2 PatrolOffset { get; protected set; }
        
        private LayerMask _groundsLayerMask;
        private Vector2 _patrolPoint1;
        private Vector2 _patrolPoint2;

        public Vector2 NextPatrolPoint { get; private set; }

        protected override void OnInitiation()
        {
            base.OnInitiation();

            Vector3 offset = new(PatrolOffset.x, PatrolOffset.y, 0);
            _patrolPoint1 = Position + offset;
            _patrolPoint2 = Position - offset;
            NextPatrolPoint = _patrolPoint1;

            _groundsLayerMask = LayerMask.GetMask("Background");
        }

        protected override void OnUpdate()
        {
            //animator.UpdateValues(rigidbody.velocity);
        }

        public void SwitchPatrolPoint() => NextPatrolPoint = NextPatrolPoint == _patrolPoint1 ? _patrolPoint2 : _patrolPoint1;

        public bool TryMoveTo(Vector2 direction, float speed)
        {
            CheckStepText = $"dir {direction}. {CheckStep(direction, Position)}";
            if (direction.magnitude < 0.05f || !CheckStep(direction, Position)) return false;
            else
            {
                Mover?.SetInput(direction, speed);
                return true;
            }
        }

        protected virtual bool CheckStep(Vector2 direction, Vector3 origin)
        {
            Vector3 checkDirection = new(direction.x, direction.y, 0);
            bool res = true;

            Debug.DrawRay(origin, new(direction.x, 0, 0), Color.red, 1f);
            Debug.DrawRay(origin, new(direction.x, -0.5f, 0), Color.green, 1f);

            if (Physics.Raycast(origin, new(direction.x, 0, 0), 0.5f)) res = false;
            //if (!Physics.Raycast(origin, new(direction.x, -0.5f, 0), 1.2f, _groundsLayerMask)) res = false;
            return res;
        }

        public virtual bool IsPointAccessable(Vector2 direction)
        {
            float sensitivity = 0.5f;
            Vector3 step = direction.normalized * sensitivity;
            int steps = Mathf.RoundToInt(direction.magnitude / sensitivity);

            bool res = true;
            for(int i = 1; i < steps; i++)
                if(!CheckStep(direction, Position + step*i))
                {
                    res = false;
                    break;
                }

            return res;
        }

        public override void SetTargetDirection(Vector3 direction)
        {
            XDirection = visualBody.right.x;
        }

        public void DisplayAttack(Action onAttackPositionReady) =>
            animator.TriggerAnimation(ActionState.Attack, () => onAttackPositionReady());

    }
}