using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GameModel
    {
        public int Score { get; private set; }
        public List<Location> Locations { get; private set; }

        internal PlayerModel PlayerModel { get; private set; }

        public GameModel(PlayerSavedData data, LevelObjectConfig playerConfig)
        {
            data.LastEntryDate++;

            PlayerModel = new(data, playerConfig);

            Score = data.Score;

            Locations = data.Locations;
        }


        public PlayerSavedData GetSaveData()
        {
            var data = PlayerModel.SaveData;

            data.Score = Score;
            data.Locations = Locations;

            return data;
        }

        public void AddScore(int score) => Score += score;

        public GroundsModel GetGroundsModel(LevelObjectFactory factory) => new GroundsModel(CalculateGroundsLenght(), factory);

        private int CalculateGroundsLenght() => 20;
    }
}
