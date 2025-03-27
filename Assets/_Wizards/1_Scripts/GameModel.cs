using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GameModel
    {
        private const float DAILY_SCORE_REDUCTION = 0.1f;

        private Location _activeLocation;

        public List<Location> Locations { get; private set; }

        internal PlayerModel PlayerModel { get; private set; }

        public GameModel(PlayerSavedData data, LevelObjectConfig playerConfig)
        {
            PlayerModel = new(data, playerConfig);

            Locations = data.Locations;

            AdjustForDate();
        }

        private void AdjustForDate()
        {
            //replace with actual delta date
            int deltaDate = 1;

            foreach (var l in Locations)
            {
                l.SetBaseScore();
                for (int i = 0; i < deltaDate; i++)
                {
                    int red = Mathf.RoundToInt(DAILY_SCORE_REDUCTION * l.Score);
                    if (red < 1) red = 1;
                    l.AccountForScore(-red);
                }
            }

            PlayerModel.SaveData.LastEntryDate++;
        }

        public PlayerSavedData GetSaveData()
        {
            var data = PlayerModel.SaveData;

            data.Locations = Locations;

            return data;
        }

        public void SetActiveLocation(Location location) => _activeLocation = location;

        public void AddScore(int score) => _activeLocation?.AccountForScore(score);

        public GroundsModel GetGroundsModel(LevelObjectFactory factory) => new GroundsModel(CalculateGroundsLenght(), factory);

        private int CalculateGroundsLenght() => 20;
    }
}
