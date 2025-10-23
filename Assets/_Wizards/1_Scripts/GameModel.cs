using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GameModel
    {
        private const float DAILY_SCORE_REDUCTION = 0.1f;
        public const int LEVEL_SCORE_MULTIPLIER = 10;

        private Location _activeLocation;

        public List<Location> Locations { get; private set; }

        internal PlayerModel PlayerModel { get; private set; }

        public GameModel(PlayerSavedData data, LevelObjectConfig playerConfig)
        {
            PlayerModel = new(data, playerConfig);

            Locations = data.Locations;

            AdjustForDate();

            data.LastEntryDate++;
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
        }

        public PlayerSavedData GetSaveData()
        {
            var data = PlayerModel.GetSaveData();

            data.Locations = Locations;

            return data;
        }

        public void SetActiveLocation(Location location) => _activeLocation = location;

        public void AddLevelScore(float score)
        {
            int accounted = Mathf.RoundToInt(score * LEVEL_SCORE_MULTIPLIER);
            if (accounted < 0)
            {
                PlayerModel.Mastery.Change(accounted);
            }
            else
            {
                bool allMastered = true;
                foreach (var l in Locations)
                {
                    allMastered = l.Score >= 100;
                    if (!allMastered) break;
                }
                if (allMastered)
                    PlayerModel.Mastery.Change(Mathf.RoundToInt(score * LEVEL_SCORE_MULTIPLIER / Locations.Count));
            }
            _activeLocation.AccountForScore(accounted);
            PlayerModel.OnValuesChanged?.Invoke();
        }

        public LevelConfig GetLevelConfig() => _activeLocation.GetLevelConfig(CalculateDifficultyLevel());


        private int CalculateDifficultyLevel()
        {
            int res = PlayerModel.Mastery.Grade;
            res += Mathf.FloorToInt(_activeLocation.Score / LEVEL_SCORE_MULTIPLIER);
            return res;
        }
    }
}
