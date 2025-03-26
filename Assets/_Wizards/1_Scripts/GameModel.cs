using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GameModel
    {
        private PlayerSavedData _playerData;

        public bool Loaded { get; private set; } = false;
        public BonusStats Bonuses { get; private set; }
        public int Score { get; private set; }

        internal PlayerModel PlayerModel { get; private set; }

        public GameModel()
        {
            PlayerModel = new();
            Bonuses = new BonusStats(false);
        }

        public void ApplyData(PlayerSavedData data)
        {
            Loaded = true;
            _playerData = data;
            _playerData.LastEntryDate++;

            PlayerModel.ApplyData(data);
            Bonuses[BonusType.coin] = data.Bonuses;
            Score = data.Score;
        }

        public PlayerSavedData GetData()
        {
            _playerData.Score = Score;
            _playerData.Bonuses = Bonuses[BonusType.coin];
            return _playerData;
        }

        public void AddBonus(BonusType type, int value) => Bonuses[type] += value;
        public void AddScore(int score) => Score += score;

        public GroundsModel GetGroundsModel(LevelObjectFactory factory) => new GroundsModel(CalculateGroundsLenght(), factory);

        private int CalculateGroundsLenght() => 20;
    }
}
