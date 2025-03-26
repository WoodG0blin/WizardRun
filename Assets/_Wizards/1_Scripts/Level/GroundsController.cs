using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface ILevelEventAccounter
    {
        Action<Vector3> OnPlayerPositionChange { get; set; }
        void AccountForBonus(BonusType type, int value);
        void AccountForDamage(int damage);
        void SetLevelCleared();
    }

    internal class GroundsController : ILevelEventAccounter
    {
        private readonly GroundsModel _groundsModel;
        private readonly IGroundsView _groundsView;

        private int _levelObjectsCurrentHealth;
        private Action onLevelCleared;

        public Dictionary<BonusType, int> BonusesCollected { get; private set; }

        public float LevelHealthValue => (float)_levelObjectsCurrentHealth / _groundsModel.TotalHealth;
        public Action OnLevelClearanceChanged { get; set; }

        public Action<Vector3> OnPlayerPositionChange { get; set; }

        public Action<int> OnCoinsCountChange { get; set; }


        public GroundsController(GroundsModel groundsModel, IGroundsView groundsView, GroundsConfig config, Action onGroundsCleared)
        {
            _groundsModel = groundsModel;

            _groundsView = groundsView;

            onLevelCleared = onGroundsCleared;

            BonusesCollected = new();

            _groundsView.InitTiles3D(config.Block);

            foreach (var lo in _groundsModel.LevelObjects) lo.SetSubscriptions(this);

            _levelObjectsCurrentHealth = _groundsModel.TotalHealth;

            _groundsView.DrawGrounds(_groundsModel.Grid, _groundsModel.LevelObjects);
        }


        public void UpdatePlayerposition(Vector3 newPosition) => OnPlayerPositionChange?.Invoke(newPosition);
        public void ClearBonuses() => BonusesCollected = new();


        void ILevelEventAccounter.AccountForBonus(BonusType type, int value)
        {
            if(!BonusesCollected.ContainsKey(type)) BonusesCollected.Add(type, 0);

            BonusesCollected[type] += value;

            if (type == BonusType.coin) OnCoinsCountChange?.Invoke(BonusesCollected[BonusType.coin]);
        }
        
        void ILevelEventAccounter.AccountForDamage(int damage)
        {
            _levelObjectsCurrentHealth -= damage;
            OnLevelClearanceChanged?.Invoke();
        }

        void ILevelEventAccounter.SetLevelCleared() => onLevelCleared?.Invoke();
    }
}
