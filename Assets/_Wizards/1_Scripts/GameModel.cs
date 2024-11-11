using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GameModel
    {
        public BonusStats Bonuses { get; private set; }
        internal InventoryModel InventoryModel { get; }

        internal PlayerModel PlayerModel { get; private set; }

        public GameModel()
        {
            InventoryModel = new InventoryModel();
            PlayerModel = new();
            Bonuses = new BonusStats(true);
        }

        public void AddBonus(BonusType type, int value) => Bonuses[type] += value;

        public GroundsModel GetGroundsModel(LevelObjectFactory factory) => new GroundsModel(CalculateGroundsLenght(), factory);

        private int CalculateGroundsLenght() => 20;
    }
}
