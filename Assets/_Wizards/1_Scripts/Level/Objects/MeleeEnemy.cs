using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class MeleeEnemy : ActiveObject, IDemonStateContext
    {
        private new MeleeEnemyView view;
        private LevelObjectConfig config;

        private DemonState _currentState;

        private float _patrolDistance;
        private float _closingDistance;

        public MeleeEnemy(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            //weaponArtifact = new Weapon(config.WeaponConfig);
            //weapon = weaponArtifact.GetExecutor(ArtifactExecutorType.Attack);
            this.config = config;

            _currentState = new DemonIdle(this);

            _closingDistance = config.MainWeaponConfig.ActionDistance;
            _patrolDistance = _closingDistance * 5;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<MeleeEnemyView>();

        protected override void OnInitiateView()
        {
            view = base.view as MeleeEnemyView;
            view.Init(config);
            view.SetUpdateActions(() => _currentState.Act());

            barrel = view.Barrel;

            base.OnInitiateView();
        }

        public override void Destroy()
        {
            base.Destroy();
            //view.SetUpdateActions(null);
        }

        void IDemonStateContext.SetNewState(DemonStates state)
        {
            _currentState = (view == null ? DemonStates.None : state) switch
            {
                DemonStates.Idle => new DemonIdle(this),
                DemonStates.Patrolling => new DemonPatrolling(this),
                DemonStates.Pursuing => new DemonPursuing(this),
                _ => new DemonStub(this),
            };
            view.DisplayState(state.ToString());
        }

        bool IDemonStateContext.IsTargetInSight()
        {
            Vector3 targetDirection = currentPlayerPosition - view.Position;

            return
            (view.XDirection == Mathf.Sign(targetDirection.x)
            && Vector3.SqrMagnitude(targetDirection) < (_patrolDistance * _patrolDistance)
            && Mathf.Abs(Vector3.Dot(targetDirection.normalized, Vector3.right * view.XDirection)) > 0.8f);
        }

        bool IDemonStateContext.IsPathClear(float direction)
        {
            float dir = direction == 0 ? view.XDirection : direction;
            return view.CheckNoGap(dir) && !(dir > 0 ? view.AccessContacts().HasContactRight : view.AccessContacts().HasContactLeft);
        }

        int IDemonStateContext.GetPatrolDirection()
        {
            float moveDirection = view.PatrolPoint.x - view.Position.x;

            if(Mathf.Abs(moveDirection) < _patrolDistance) moveDirection = view.XDirection;

            return moveDirection < 0 ? -1 : 1;
        }

        int IDemonStateContext.GetPursuingDirection()
        {
            float targetDirection = currentPlayerPosition.x - view.Position.x;

            if (isInAttackDistance(targetDirection)) return 0;
            else return targetDirection < 0 ? -1 : 1;
        }
        private bool isInAttackDistance(float targetDirection) =>
            targetDirection * view.XDirection >= 0 && Mathf.Abs(targetDirection) < _closingDistance;

        void IDemonStateContext.Move(float direction) =>
            view.Mover?.SetInput(new(direction, 0), stats.Speed);

        void IDemonStateContext.Fire()
        {
            Direction = new(view.XDirection, 0);
            if (weapon.IsReady)
                view.DisplayAttack(weapon.Use);
        }

        void IDemonStateContext.SetWait(float time, Action onFinish) => 
            view.StartCoroutine(Wait(time, onFinish));

        private IEnumerator Wait(float seconds, Action onFinish)
        {
            yield return new WaitForSeconds(seconds);
            onFinish?.Invoke();
        }

        void IDemonStateContext.FlipDirection() =>
            view.Mover.SetInput(new(-view.XDirection * 0.01f, 0));
    }


    internal enum DemonStates { None, Idle, Patrolling, Pursuing, Attacking }


    internal interface IDemonStateContext
    {
        void SetNewState(DemonStates newState);
        void SetWait(float time, Action onFinish);

        bool IsTargetInSight();
        bool IsPathClear(float direction);

        int GetPatrolDirection();
        int GetPursuingDirection();
        void FlipDirection();

        void Move(float direction);
        void Fire();
    }


    internal abstract class DemonState
    {
        protected IDemonStateContext context;

        public DemonState(IDemonStateContext context) => this.context = context;

        public void Act() => OnAction();
        protected abstract void OnAction();

    }

    internal class DemonStub : DemonState
    {
        public DemonStub(IDemonStateContext context) : base(context) { }
        protected override void OnAction() { }
    }

    internal class DemonIdle : DemonState
    {
        private float _timeIdle = 1f;
        private bool _idling = false;

        public DemonIdle(IDemonStateContext context) : base(context) { }

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
            context.SetNewState(context.IsTargetInSight() ? DemonStates.Pursuing : DemonStates.Patrolling);
            _idling = false;
        }
    }

    internal class DemonPatrolling : DemonState
    {
        public DemonPatrolling(IDemonStateContext context) : base(context) { }

        protected override void OnAction()
        {
            if (context.IsTargetInSight()) context.SetNewState(DemonStates.Pursuing);
            else
            {
                int moveDirection = context.GetPatrolDirection();
                
                if (context.IsPathClear(moveDirection)) context.Move(moveDirection);
                else context.SetNewState(DemonStates.Idle);
            }
        }
    }

    internal class DemonPursuing : DemonState
    {
        public DemonPursuing(IDemonStateContext context) : base(context) { }

        protected override void OnAction()
        {
            if (!context.IsTargetInSight() || !context.IsPathClear(0)) context.SetNewState(DemonStates.Idle);
            else
            {
                int direction = context.GetPursuingDirection();

                if (direction == 0) context.Fire();
                else context.Move(direction);
            }
        }
    }
}
