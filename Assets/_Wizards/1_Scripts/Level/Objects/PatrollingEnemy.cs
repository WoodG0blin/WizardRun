using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{

    internal class PatrollingEnemy : StandingEnemy, IMovingStateContext
    {
        private new PatrollingEnemyView view;

        private BaseMovingState _currentState;

        private float _sensingDistance;
        private float _closingDistance;

        public PatrollingEnemy(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            _currentState = new IdleState(this);

            _closingDistance = this.config.Ammo.ActionRange;
            _sensingDistance = _closingDistance * 5;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<PatrollingEnemyView>();

        protected override void OnInitiateView()
        {
            base.OnInitiateView();
            view = base.view as PatrollingEnemyView;
        }

        protected override void ActionsOnUpdate()
        {
            base.ActionsOnUpdate();
            _currentState.Act();
            view.Message = $"Direction: {TargetDirection}. Weapon check: {Weapon.CheckAction(TargetDirection)}";
        }
        protected override void ExecuteAttackAction()
        {
            view.DisplayAttack(() => Weapon.Use(this));
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
            view.StateText = state.ToString();
        }

        Vector2 IMovingStateContext.TargetApproachDirection
        { get
            {
                var dist = TargetDirection;
                float closeCoeff = dist.magnitude / _closingDistance;
                if (closeCoeff > 1) dist *= closeCoeff;
                return dist;
            }
        }
        Vector2 IMovingStateContext.NextPatrolDirection => view.NextPatrolPoint - (Vector2)view.Position;
        void IMovingStateContext.SwitchPatrolPoint() => view.SwitchPatrolPoint();
        bool IMovingStateContext.IsTargetInSight => TargetDirection.magnitude < _sensingDistance && view.IsPointAccessable(TargetDirection);
        bool IMovingStateContext.TryMove(Vector2 target) => view.TryMoveTo(target, Stats.Speed);

        void IMovingStateContext.FlipDirection() => view.Mover.SetInput(new(-LookDirection * 0.01f, 0));

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
    }


    internal enum MovingStates { None, Idle, Patrolling, Pursuing, Attacking }


    internal interface IMovingStateContext
    {
        void SetNewState(MovingStates newState);
        void SetWait(float time, Action onFinish);

        bool IsTargetInSight { get; }
        Vector2 TargetApproachDirection { get; }
        Vector2 NextPatrolDirection { get; }


        bool TryMove(Vector2 target);
        void SwitchPatrolPoint();
        void FlipDirection();
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
            context.SetNewState(context.IsTargetInSight ? MovingStates.Pursuing : MovingStates.Patrolling);
            _idling = false;
        }
    }

    internal class PatrollingState : BaseMovingState
    {
        public PatrollingState(IMovingStateContext context) : base(context) { }

        protected override void OnAction()
        {
            if (context.IsTargetInSight)
                context.SetNewState(MovingStates.Pursuing);
            else
                if (!context.TryMove(context.NextPatrolDirection))
                {
                    context.SwitchPatrolPoint();
                    context.SetNewState(MovingStates.Idle);
                }
        }
    }

    internal class PursuingState : BaseMovingState
    {
        public PursuingState(IMovingStateContext context) : base(context) { }

        protected override void OnAction()
        {
            var direction = context.TargetApproachDirection;
            if (!context.TryMove(direction))
                if(!(direction.magnitude < 0.05f))
                    context.SetNewState(MovingStates.Idle);
        }
    }
}
