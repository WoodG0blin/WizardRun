using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface ILevelEventAccounter
    {
        Action<Vector2> OnPlayerPositionChange { get; set; }
        Action OnExitAvailable { get; set; }
        void AccountForBonus(Bonus bonus);
        void AccountForDamage(int damage);
        void SetLevelCleared();
        SquaresGrid CopyGrid();
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
        public int TotalDamageReceived { get; private set; } = 0;
        public Action<float> OnLevelClearanceChanged { get; set; }
        public Action OnExitAvailable { get; set; }

        public Action<Vector2> OnPlayerPositionChange { get; set; }

        public Action<BonusType, int> OnBonusCollected { get; set; }


        public GroundsController(GroundsModel groundsModel, IGroundsView groundsView, Action onGroundsCleared)
        {
            _groundsModel = groundsModel;

            _groundsView = groundsView;

            onLevelCleared = onGroundsCleared;

            ClearBonuses();

            foreach (var lo in _groundsModel.LevelObjects) lo.SetSubscriptions(this);

            _levelObjectsCurrentHealth = _groundsModel.TotalHealth;

            _groundsView.DrawGrounds(_groundsModel.Grid, _groundsModel.LevelObjects);
        }

        public void SetCamera(CameraController camera)
        {
            _cameraController= camera;
            OnPlayerPositionChange += _cameraController.UpdateToPlayerPosition;
        }

        public void ClearBonuses() => BonusesCollected = new()
            {
                { BonusType.Coin, 0},
                { BonusType.Souls, 0},
                { BonusType.Artifacts, 0}
            };


        void ILevelEventAccounter.AccountForBonus(Bonus bonus)
        {
            if(!BonusesCollected.ContainsKey(bonus.Type)) BonusesCollected.Add(bonus.Type, 0);

            BonusesCollected[bonus.Type] += bonus.Value;

            OnBonusCollected?.Invoke(bonus.Type, BonusesCollected[bonus.Type]);
        }
        
        void ILevelEventAccounter.AccountForDamage(int damage)
        {
            _levelObjectsCurrentHealth -= damage;
            TotalDamageReceived += damage;
            OnLevelClearanceChanged?.Invoke((float)TotalDamageReceived / _groundsModel.TotalHealth);
        }

        void ILevelEventAccounter.SetLevelCleared() => onLevelCleared?.Invoke();

        public SquaresGrid CopyGrid() => _groundsModel.GetGridClone();
    }
}
