using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GameModel
    {
        private Location _activeLocation;

        public List<Location> Locations { get; private set; }

        internal PlayerModel PlayerModel { get; private set; }

        public GameModel(PlayerSavedData data, LevelObjectConfig playerConfig)
        {
            data.LastEntryDate++;

            PlayerModel = new(data, playerConfig);

            Locations = data.Locations;
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
