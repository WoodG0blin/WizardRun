using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class FlyingEnemy : PatrollingEnemy
    {
        private new FlyingEnemyView view;

        private SquaresGrid _grid;
        private Stack<Vector2> _curentPath = new();
        private Vector2 _targetedPlayerPosition;
        private bool _isPursuing;

        private float _playermoveThreshold = 2f;

        public FlyingEnemy(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            sensingDistance = closingDistance * 7;
        }

        protected override void OnInitiateView()
        {
            base.OnInitiateView();
            if (base.view is FlyingEnemyView v) view = v;
        }

        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            base.SetSubscriptions(subscriber);
            _grid = subscriber.CopyGrid();
            _curentPath.Push(view.Position);
        }

        protected override void OnNewState(MovingStates state)
        {
            base.OnNewState(state);
            _isPursuing = state == MovingStates.Pursuing;
            if (state == MovingStates.Patrolling)
                _curentPath = _grid.GetPath(view.Position, view.NextPatrolPoint);
        }

        protected override Vector2 SetTargetApproachDirection()
        {
            if ((view.Position - currentPlayerPosition).magnitude < closingDistance) return view.Position;
            else return currentPlayerPosition + Vector2.one * closingDistance; 
        }
        protected override Vector2 CalculateNextStep(Vector2 targetPoint) => _curentPath.Pop();

        protected override void UpdatePlayerPosition(Vector2 playerPosition)
        {
            base.UpdatePlayerPosition(playerPosition);
            if(_isPursuing && (_targetedPlayerPosition - currentPlayerPosition).magnitude > _playermoveThreshold)
            {
                _targetedPlayerPosition = currentPlayerPosition;
                _curentPath = _grid.GetPath(view.Position, _targetedPlayerPosition);
            }
        }
    }
}
