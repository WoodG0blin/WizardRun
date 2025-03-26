using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GroundsController
    {
        private readonly GroundsModel _groundsModel;
        private readonly IGroundsView _groundsView;

        private int _levelObjectsMaxHealth;
        private int _levelObjectsCurrentHealth;

        public float LevelClearedValue => (float)_levelObjectsCurrentHealth / _levelObjectsMaxHealth;
        public Action OnLevelClearanceChanged { get; set; }



        private Action<Vector3> _onPlayerPositionChanged;

        public Dictionary<BonusType, int> BonusesCollected { get; private set; }
        public Action<int> OnCoinsCountChange { get; set; }

        public GroundsController(GroundsModel groundsModel, IGroundsView groundsView, GroundsConfig config, Action onGroundsCleared)
        {
            _groundsModel = groundsModel;

            _groundsView = groundsView;

            BonusesCollected = new();

            _groundsView.InitTiles3D(config.Block);

            foreach(var lo in _groundsModel.LevelObjects)
            {
                _levelObjectsMaxHealth += lo.MaxHealth;
                lo.AccountForDamage = AccountForDamage;

                if (lo is IPlayerPositionObserver o) _onPlayerPositionChanged += o.SetNewPlayerPosition;
                if (lo is IBonusGenerator b) b.OnBonusCollect = SetBonus;
                if (lo is IPortal p) p.onPortalEnter = onGroundsCleared;
            }

            _levelObjectsCurrentHealth = _levelObjectsMaxHealth;

            _groundsView.DrawGrounds(_groundsModel.Grid, _groundsModel.LevelObjects);
        }

        private void AccountForDamage(int damage)
        {
            _levelObjectsCurrentHealth -= damage;
            OnLevelClearanceChanged?.Invoke();
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
