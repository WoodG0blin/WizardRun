using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface ILevelEventAccounter
    {
        Action<Vector3> OnPlayerPositionChange { get; set; }
        Action OnExitAvailable { get; set; }
        void AccountForBonus(Bonus bonus);
        void AccountForDamage(int damage);
        void SetLevelCleared();
    }

    internal class GroundsController : ILevelEventAccounter
    {
        private readonly GroundsModel _groundsModel;
        private readonly IGroundsView _groundsView;

        private CameraController _cameraController;

        private int _levelObjectsCurrentHealth;
        private Action onLevelCleared;

        public Dictionary<BonusType, int> BonusesCollected { get; private set; }

        public float LevelHealthValue => (float)_levelObjectsCurrentHealth / _groundsModel.TotalHealth;
        public Action OnLevelClearanceChanged { get; set; }
        public Action OnExitAvailable { get; set; }

        public Action<Vector3> OnPlayerPositionChange { get; set; }

        public Action<int> OnCoinsCountChange { get; set; }


        public GroundsController(GroundsModel groundsModel, IGroundsView groundsView, LevelConfig config, Action onGroundsCleared)
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

        public void SetCamera(CameraController camera)
        {
            _cameraController= camera;
            OnPlayerPositionChange += _cameraController.UpdateToPlayerPosition;
        }

        public void ClearBonuses() => BonusesCollected = new();


        void ILevelEventAccounter.AccountForBonus(Bonus bonus)
        {
            if(!BonusesCollected.ContainsKey(bonus.Type)) BonusesCollected.Add(bonus.Type, 0);

            BonusesCollected[bonus.Type] += bonus.Value;

            if (bonus.Type == BonusType.Coin) OnCoinsCountChange?.Invoke(BonusesCollected[BonusType.Coin]);
        }
        
        void ILevelEventAccounter.AccountForDamage(int damage)
        {
            _levelObjectsCurrentHealth -= damage;
            OnLevelClearanceChanged?.Invoke();
        }

        void ILevelEventAccounter.SetLevelCleared() => onLevelCleared?.Invoke();
    }
}
