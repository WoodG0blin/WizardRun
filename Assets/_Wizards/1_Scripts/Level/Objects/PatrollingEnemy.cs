using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardsPlatformer
{

    internal class PatrollingEnemy : ActiveObject, IMovingStateContext
    {
        private new MeleeEnemyView view;

        private BaseMovingState _currentState;

        private float _patrolDistance;
        private float _closingDistance;

        public PatrollingEnemy(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            //weaponArtifact = new Weapon(config.WeaponConfig);
            //weapon = weaponArtifact.GetExecutor(ArtifactExecutorType.Attack);

            _currentState = new IdleState(this);

            _closingDistance = this.config.Ammo.ActionRange;
            _patrolDistance = _closingDistance * 5;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<MeleeEnemyView>();

        protected override void OnInitiateView()
        {
            view = base.view as MeleeEnemyView;
            view.Init(config);
            view.SetUpdateActions(SetAttack);
            view.AddUpdateActions(() => _currentState.Act());
            view.AddUpdateActions(() => view.DisplayMessage($"Direction: {Direction}. Weapon check: {Weapon.CheckAction(Direction)}"));

            Barrel = view.Barrel;

            base.OnInitiateView();
        }

        public override void Destroy()
        {
            base.Destroy();
            //view.SetUpdateActions(null);
        }

        void IMovingStateContext.SetNewState(MovingStates state)
        {
            _currentState = (view == null ? MovingStates.None : state) switch
            {
                MovingStates.Idle => new IdleState(this),
                MovingStates.Patrolling => new PatrollingState(this),
                MovingStates.Pursuing => new PursuingState(this),
                _ => new StubMovingState(this),
            };
            view.DisplayState(state.ToString());
        }

        bool IMovingStateContext.IsTargetInSight()
        {
            Vector3 targetDirection = currentPlayerPosition - view.Position;

            return
            (view.XDirection == Mathf.Sign(targetDirection.x)
            && Vector3.SqrMagnitude(targetDirection) < (_patrolDistance * _patrolDistance)
            && Mathf.Abs(Vector3.Dot(targetDirection.normalized, Vector3.right * view.XDirection)) > 0.8f);
        }

        bool IMovingStateContext.IsPathClear(float direction)
        {
            float dir = direction == 0 ? view.XDirection : direction;
            return view.CheckNoGap(dir) && !(dir > 0 ? view.AccessContacts().HasContactRight : view.AccessContacts().HasContactLeft);
        }

        int IMovingStateContext.GetPatrolDirection()
        {
            float moveDirection = view.PatrolPoint.x - view.Position.x;

            if(Mathf.Abs(moveDirection) < _patrolDistance) moveDirection = view.XDirection;

            return moveDirection < 0 ? -1 : 1;
        }

        int IMovingStateContext.GetPursuingDirection()
        {
            float targetDirection = currentPlayerPosition.x - view.Position.x;

            if (isInAttackDistance(targetDirection)) return 0;
            else return targetDirection < 0 ? -1 : 1;
        }
        private bool isInAttackDistance(float targetDirection) =>
            targetDirection * view.XDirection >= 0 && Mathf.Abs(targetDirection) < _closingDistance;

        void IMovingStateContext.Move(float direction)
        {
            view.Mover?.SetInput(new(direction, 0), Stats.Speed);
        }

        void IMovingStateContext.Fire()
        {
            Direction = new(view.XDirection, 0);
            if (Weapon.IsReady)
                view.DisplayAttack(() => Weapon.Use(this));
        }

        void IMovingStateContext.SetWait(float time, Action onFinish) => 
            view.StartCoroutine(Wait(time, onFinish));

        private IEnumerator Wait(float seconds, Action onFinish)
        {
            float elapsed = 0;
            while(elapsed < seconds)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            onFinish?.Invoke();
        }

        void IMovingStateContext.FlipDirection() =>
            view.Mover.SetInput(new(-view.XDirection * 0.01f, 0));
    }


    internal enum MovingStates { None, Idle, Patrolling, Pursuing, Attacking }


    internal interface IMovingStateContext
    {
        void SetNewState(MovingStates newState);
        void SetWait(float time, Action onFinish);

        bool IsTargetInSight();
        bool IsPathClear(float direction);

        int GetPatrolDirection();
        int GetPursuingDirection();
        void FlipDirection();

        void Move(float direction);
        void Fire();
    }


    internal abstract class BaseMovingState
    {
        protected IMovingStateContext context;

        public BaseMovingState(IMovingStateContext context) => this.context = context;

        public void Act() => OnAction();
        protected abstract void OnAction();

    }

    internal class StubMovingState : BaseMovingState
    {
        public StubMovingState(IMovingStateContext context) : base(context) { }
        protected override void OnAction() { }
    }

    internal class IdleState : BaseMovingState
    {
        private float _timeIdle = 1f;
        private bool _idling = false;

        public IdleState(IMovingStateContext context) : base(context) { }

        protected override void OnAction()
        {
            if (!_idling)
            {
                context.SetWait(_timeIdle, OnFinishIdle);
                _idling = true;
            }
        }

        private void OnFinishIdle()
        {
            context.FlipDirection();
            context.SetNewState(context.IsTargetInSight() ? MovingStates.Pursuing : MovingStates.Patrolling);
            _idling = false;
        }
    }

    internal class PatrollingState : BaseMovingState
    {
        public PatrollingState(IMovingStateContext context) : base(context) { }

        protected override void OnAction()
        {
            if (context.IsTargetInSight()) context.SetNewState(MovingStates.Pursuing);
            else
            {
                int moveDirection = context.GetPatrolDirection();
                if (context.IsPathClear(moveDirection)) context.Move(moveDirection);
                else context.SetNewState(MovingStates.Idle);
            }
        }
    }

    internal class PursuingState : BaseMovingState
    {
        public PursuingState(IMovingStateContext context) : base(context) { }

        protected override void OnAction()
        {
            if (!context.IsTargetInSight() || !context.IsPathClear(0)) context.SetNewState(MovingStates.Idle);
            else
            {
                int direction = context.GetPursuingDirection();

                //if (direction == 0) context.Fire();
                //else context.Move(direction);
                context.Move(direction);
            }
        }
    }
}
