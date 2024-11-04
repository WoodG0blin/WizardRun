using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal enum GameState { Game, MainMenu, Settings, Inventory, Shop, Exit }
    internal class GameModel
    {
        public SubscribtableProperty<GameState> CurrentState { get; }
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

        public GroundsModel GetGroundsModel() => new GroundsModel(CalculateGroundsLenght());

        private int CalculateGroundsLenght() => 20;
    }
}
