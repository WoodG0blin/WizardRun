using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GroundsController : Controller
    {
        private readonly GroundsModel _groundsModel;
        private readonly GroundsView _groundsView;

        private SubscribtableProperty<Vector3> _playerPosition;

        public Dictionary<BonusType, int> BonusesCollected { get; private set; }
        public Action<int> OnCoinsCountChange;

        public GroundsController(GroundsModel groundsModel, GroundsView groundsView, GroundsConfig config, Action onGroundsCleared)
        {
            _groundsModel = groundsModel;

            _groundsView = groundsView;

            _playerPosition = new();
            BonusesCollected = new();

            _groundsView.InitTiles(config.GroundTiles);

            _groundsView.DrawGrounds(_groundsModel.Grid, _groundsModel.LevelObjects, config.LevelObjectsRepository.Items);
            _groundsView.Init(
                _playerPosition,
                onGroundsCleared,
                SetBonus);
        }

        public Vector3 GetStartPosition() => _groundsView.GetGlobalStartPosition(_groundsModel.LocalStartPosition);
        public void SetActive(bool active) => _groundsView.SetActive(active);
        public void UpdatePlayerposition(Vector3 newPosition) => _playerPosition.Value = newPosition;
        public void ClearBonuses() => BonusesCollected = new();

        private void SetBonus(BonusType type, int value)
        {
            if(!BonusesCollected.ContainsKey(type)) BonusesCollected.Add(type, 0);

            BonusesCollected[type] += value;

            if (type == BonusType.coin) OnCoinsCountChange?.Invoke(BonusesCollected[BonusType.coin]);
        }
    }
}
