using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace WizardsPlatformer
{
    internal class DemonView : LevelObjectView, IPlayerPositionObserver, IAnimated, IKillable
    {
        //TODO set into config
        private const float _patrolDistance = 3f;

        public enum DemonStates { Idle, Patrolling, Pursuing, Attacking }

        private AnimationController _animator;
        public ActionState animationState { get; set; }

        private DemonState _currentState;
        private LayerMask _layerMask;

        private IWeapon _weapon;

        SubscribtableProperty<Vector3> _playerPositionProperty;

        public Vector3 PositionVector { get; private set; }
        public Vector3 TargetPosition { get; private set; }
        public float MoveDirection { get; private set; }

        public Vector3 PatrolPoint { get; private set; }
        public float PatrolDistance { get; private set; }
        public bool IsAttackReady => _weapon.WeaponReady;

        private float _speed;
        private float _closingDistance;

        private int _bonusesOnKill;

        public event Action<float> OnReceiveDamage;
        public event Action<BonusType ,int> OnKilled;

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

            if(state == DemonStates.Idle) _animator?.AnimationState(ActionState.Idle, true);
            else _animator?.AnimationState(ActionState.Walk);
        }


        protected override void OnInit()
        {
            activeResponse = true;

            PatrolPoint = Position;
            MoveDirection = XDirection;
            _speed = config.Speed;

            PatrolDistance = _patrolDistance;

            if (config.HasWeapon)
            {
                _weapon = Weapon.GetWeapon(transform, config.WeaponConfig);
                _closingDistance = config.WeaponConfig.AttackDistance;
            }

            stats = new Stats(health: config.MaxHealth, parent: transform);
            stats.OnDeath += OnDeath;

            _bonusesOnKill = config.BonusesOnKill;

            _layerMask = LayerMask.GetMask("Background");
            RegisterOnUpdate();

            SetNewState(DemonStates.Idle);
        }

        public void InitiateAnimations(AnimationSequence[] animations)
        {
            _animator = new AnimationController(
                renderer,
                animations ?? new AnimationSequence[] { new AnimationSequence() { Sprites = new List<Sprite>() { renderer.sprite } } }
                );
        }

        public void RegisterObserveTarget(SubscribtableProperty<Vector3> observeTarget)
        {
            _playerPositionProperty = observeTarget;
            _playerPositionProperty.SubscribeOnValueChange(OnPlayerPositionChange);
        }

        protected override void OnUpdate()
        {
            PositionVector = Position;
            _currentState.Act();

            _animator?.Update();
        }

        public bool PathClear { get => NoGap(MoveDirection) && HasNoSideBarriers(MoveDirection); }

        private bool HasNoSideBarriers(float XDirection) =>
            (XDirection < 0 && !AccessContacts().HasContactLeft) || (XDirection > 0 && !AccessContacts().HasContactRight);

        private bool NoGap(float XDirection)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                        new Vector2(PositionVector.x, PositionVector.y),
                        new Vector2(XDirection, -1),
                        1.5f, _layerMask);

            return hit.collider != null;
        }

        public bool IsInPatrolDistance { get => Mathf.Abs((PositionVector - PatrolPoint).x) < PatrolDistance; }

        public bool TargetInSight
        {
            get =>
            (MoveDirection == Mathf.Sign((TargetPosition - PositionVector).x)
            && Vector3.SqrMagnitude(TargetPosition - PositionVector) < (_patrolDistance * _patrolDistance)
            && Mathf.Abs(Vector3.Dot(TargetPosition - PositionVector, Vector3.right * MoveDirection)) > 0.8f);
        }

        public bool IsInAttackDistance
        {
            get
            {
                var temp = TargetPosition.x - PositionVector.x;
                if(temp * MoveDirection >= 0 && Mathf.Abs(temp) < _closingDistance) return true;
                return false;
            }
        }

        public void Move() => rigidbody.velocity = new Vector2(MoveDirection, 0) * _speed;

        public void FlipDirection()
        {
            MoveDirection *= -1;
            SetDirection(MoveDirection);
        }

        public void StopMoving() => rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);

        protected void OnPlayerPositionChange(Vector3 newPlayerPosition) => TargetPosition = newPlayerPosition;

        protected override void OnDestruction()
        {
            _playerPositionProperty.UnsubscribeOnValueChange(OnPlayerPositionChange);
            _playerPositionProperty = null;
            stats.OnDeath -= OnDeath;
        }

        public void Fire(Vector3 direction)
        {
            _weapon.SetDirection(direction);
            if (_weapon.WeaponReady)
            {
                _animator?.AnimationState(ActionState.Attack, true);
                _weapon.Fire();
            }
        }

        public void ReceiveDamage(float damage) => stats.Health -= damage;

        private void OnDeath()
        {
            OnKilled?.Invoke(BonusType.coin ,_bonusesOnKill);
            _currentState.Stop();
            UnRegisterFromUpdate();
            gameObject.SetActive(false);
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
            demonView.SetNewState(demonView.TargetInSight ? DemonView.DemonStates.Pursuing : DemonView.DemonStates.Patrolling);
            stateCheck = null;
        }
    }

    internal class DemonPatrolling : DemonState
    {
        public DemonPatrolling(DemonView demonView) : base(demonView) { }

        protected override void OnAction()
        {
            Vector3 _targetPosition = demonView.PatrolPoint + demonView.MoveDirection * demonView.PatrolDistance * (demonView.IsInPatrolDistance ? Vector3.right : Vector3.left);

            if ((_targetPosition - demonView.PositionVector).x * demonView.MoveDirection < 0 || !demonView.PathClear)
            {
                demonView.FlipDirection();
                demonView.SetNewState(DemonView.DemonStates.Idle);
            }
            else demonView.Move();

            if(demonView.TargetInSight) demonView.SetNewState(DemonView.DemonStates.Pursuing);
        }
    }

    internal class DemonPursuing : DemonState
    {
        public DemonPursuing(DemonView demonView) : base(demonView) { }

        protected override void OnAction()
        {
            if (!demonView.TargetInSight) demonView.SetNewState(DemonView.DemonStates.Idle);
            else
            {
                if (!demonView.PathClear)
                {
                    if (demonView.IsInAttackDistance)
                    {
                        if(demonView.IsAttackReady) demonView.SetNewState(DemonView.DemonStates.Attacking);
                    }
                    else
                    {
                        demonView.FlipDirection();
                        demonView.SetNewState(DemonView.DemonStates.Idle);
                    }
                }
                else demonView.Move();
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
            demonView.SetNewState(DemonView.DemonStates.Pursuing);
        }
    }
}