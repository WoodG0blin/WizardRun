using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GroundsController
    {
        private readonly GroundsModel _groundsModel;
        private readonly GroundsView _groundsView;

        private Action<Vector3> _onPlayerPositionChanged;

        public Dictionary<BonusType, int> BonusesCollected { get; private set; }
        public Action<int> OnCoinsCountChange;

        public GroundsController(GroundsModel groundsModel, GroundsView groundsView, GroundsConfig config, Action onGroundsCleared)
        {
            _groundsModel = groundsModel;

            _groundsView = groundsView;

            BonusesCollected = new();

            _groundsView.InitTiles(config.GroundTiles);

            foreach(var lo in _groundsModel.LevelObjects)
            {
                if (lo is IPlayerPositionObserver o) _onPlayerPositionChanged += o.SetNewPlayerPosition;
                if (lo is IBonusGenerator b) b.OnBonusCollect = SetBonus;
                if (lo is IPortal p) p.onPortalEnter = onGroundsCleared;
            }

            _groundsView.DrawGrounds(_groundsModel.Grid, _groundsModel.LevelObjects);
            //_groundsView.Init(
            //    _playerPosition,
            //    onGroundsCleared,
            //    SetBonus);
        }

        public void UpdatePlayerposition(Vector3 newPosition) => _onPlayerPositionChanged?.Invoke(newPosition);
        public void ClearBonuses() => BonusesCollected = new();

        private void SetBonus(BonusType type, int value)
        {
            if(!BonusesCollected.ContainsKey(type)) BonusesCollected.Add(type, 0);

            BonusesCollected[type] += value;

            if (type == BonusType.coin) OnCoinsCountChange?.Invoke(BonusesCollected[BonusType.coin]);
        }
    }
}
