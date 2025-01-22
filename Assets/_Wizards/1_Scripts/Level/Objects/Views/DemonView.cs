using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardsPlatformer
{
    internal enum DemonStates { Idle, Patrolling, Pursuing, Attacking }

    internal class DemonView : LevelObjectView
    {
        [field: SerializeField] public Transform Barrel { get; private set; }

        private new Rigidbody rigidbody;

        private DemonState _currentState;
        private LayerMask _layerMask;

        public Vector3 PositionVector {get; private set; }
        public Vector3 TargetPosition { get; private set; }

        public Vector3 PatrolPoint { get; private set; }
        public float PatrolDistance { get; private set; }
        public bool IsAttackReady => IsWeaponReady();

        private float _speed;
        private float _closingDistance;


        public Action<Vector3> OnAttackReady { get; set; }
        public Func<bool> IsWeaponReady { get; set; }

        public void SetNewState(DemonStates state)
        {
            _currentState = state switch
            {
                DemonStates.Idle => new DemonIdle(this),
                DemonStates.Patrolling => new DemonPatrolling(this),
                DemonStates.Pursuing => new DemonPursuing(this),
                DemonStates.Attacking => new DemonAttacking(this),
                _ => new DemonIdle(this),
            };
        }


        public void Init(LevelObjectConfig config, Action<Vector3> onAttackReady, Func<bool> isWeaponReady)
        {
            rigidbody = transform.GetComponent<Rigidbody>();

            PatrolPoint = Position;
            _speed = config.Speed;

            _closingDistance = config.WeaponConfig.ActionDistance;
            PatrolDistance = _closingDistance * 5;

            OnAttackReady = onAttackReady;
            IsWeaponReady = isWeaponReady;

            _layerMask = LayerMask.GetMask("Background");

            Barrel ??= transform;

            SetNewState(DemonStates.Idle);
        }


        protected override void OnUpdate()
        {
            PositionVector = Position;
            _currentState.Act();

            animator.UpdateValues(rigidbody.velocity);
        }

        public bool PathClear { get => CheckNoGap(XDirection) && HasNoSideBarriers(XDirection); }

        private bool HasNoSideBarriers(float XDirection) =>
            (XDirection < 0 && !AccessContacts().HasContactLeft) || (XDirection > 0 && !AccessContacts().HasContactRight);

        public bool CheckNoGap(float XDirection) =>
            Physics.Raycast(PositionVector, new(XDirection, -0.5f, 0), 1.2f, _layerMask);

        public bool IsInPatrolDistance { get => Mathf.Abs((PositionVector - PatrolPoint).x) < PatrolDistance; }

        public bool TargetInSight
        {
            get =>
            (XDirection == Mathf.Sign((TargetPosition - PositionVector).x)
            && Vector3.SqrMagnitude(TargetPosition - PositionVector) < (PatrolDistance * PatrolDistance)
            && Mathf.Abs(Vector3.Dot((TargetPosition - PositionVector).normalized, Vector3.right * XDirection)) > 0.8f);
        }

        public bool IsInAttackDistance
        {
            get
            {
                var temp = TargetPosition.x - PositionVector.x;
                if (temp * XDirection >= 0 && Mathf.Abs(temp) < _closingDistance) return true;
                return false;
            }
        }

        public void Move() => rigidbody.velocity = new Vector3(base.XDirection, 0, 0) * _speed;

        public void FlipDirection() => SetDirection(-base.XDirection);

        public void StopMoving() => rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);

        public void SetNewPlayerPosition(Vector3 newPlayerPosition) => TargetPosition = newPlayerPosition;

        public void Fire(Vector3 direction)
        {
            if (IsAttackReady)
            {
                //OnAttackReady(direction);
                animator.TriggerAnimation(ActionState.Attack, () => OnAttackReady(direction));
            }
        }
    }


    internal abstract class DemonState
    {
        protected DemonView demonView;
        protected Coroutine stateCheck;
        public ActionState animationState { get; set; }

        protected Vector3 _targetPosition;

        public DemonState(DemonView demonView) => this.demonView = demonView;

        public void Act() => OnAction();
        public void Stop()
        {
            if(stateCheck != null) demonView.StopCoroutine(stateCheck);
        }
        protected abstract void OnAction();
    }

    internal class DemonIdle : DemonState
    {
        private float _timeIdle = 1f;
        public DemonIdle(DemonView demonView) : base(demonView) { }

        protected override void OnAction()
        {
            //demonView.StopMoving();
            stateCheck ??= demonView.StartCoroutine(OnStateChange(_timeIdle));
        }

        IEnumerator OnStateChange(float t)
        {
            yield return new WaitForSeconds(t);
            demonView.SetNewState(demonView.TargetInSight ? DemonStates.Pursuing : DemonStates.Patrolling);
            stateCheck = null;
        }
    }

    internal class DemonPatrolling : DemonState
    {
        public DemonPatrolling(DemonView demonView) : base(demonView) { }

        protected override void OnAction()
        {
            Vector3 _targetPosition = demonView.PatrolPoint + demonView.XDirection * demonView.PatrolDistance * (demonView.IsInPatrolDistance ? Vector3.right : Vector3.left);

            if ((_targetPosition - demonView.PositionVector).x * demonView.XDirection < 0 || !demonView.PathClear)
            {
                demonView.FlipDirection();
                demonView.SetNewState(DemonStates.Idle);
            }
            else demonView.Move();

            if(demonView.TargetInSight) demonView.SetNewState(DemonStates.Pursuing);
        }
    }

    internal class DemonPursuing : DemonState
    {
        public DemonPursuing(DemonView demonView) : base(demonView) { }

        protected override void OnAction()
        {
            if (!demonView.TargetInSight)
            {
                demonView.SetNewState(DemonStates.Idle);
            }
            else
            {
                if (demonView.PathClear)
                {
                    if (demonView.IsInAttackDistance)
                    {
                        if (demonView.IsAttackReady) demonView.SetNewState(DemonStates.Attacking);
                    }
                    else
                    {
                        demonView.Move();
                    }
                }
                else
                {
                    demonView.FlipDirection();
                    demonView.SetNewState(DemonStates.Idle);
                }
            }
        }
    }

    internal class DemonAttacking : DemonState
    {
        public DemonAttacking(DemonView demonView) : base(demonView) { }

        protected override void OnAction()
        {
            if(stateCheck == null)
            {
                demonView.Fire(demonView.TargetPosition - demonView.PositionVector);
                stateCheck = demonView.StartCoroutine(CoolDown(0.1f));
            }
        }

        IEnumerator CoolDown(float t)
        {
            yield return new WaitForSeconds(t);
            demonView.SetNewState(DemonStates.Pursuing);
        }
    }
}