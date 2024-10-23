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
        internal LevelModel LevelModel { get; }
        internal InventoryModel InventoryModel { get; }

        public GameModel(GameState initialState)
        {
            CurrentState = new SubscribrablePropertyWithEqualsCheck <GameState>();
            CurrentState.Value = initialState;
            InventoryModel = new InventoryModel();
            LevelModel = new LevelModel(CurrentState, AddBonus);
            //AnalyticsManager = GameObject.FindObjectOfType<AnalyticsManager>();
            //AdsManager= GameObject.FindObjectOfType<AdsManager>();
            Bonuses = new BonusStats(true);
        }

        public GameModel()
        {
            InventoryModel = new InventoryModel();
            Bonuses = new BonusStats(true);
        }

        public void AddBonus(BonusType type, int value) => Bonuses[type] += value;

        public GroundsModel GetGroundsModel() => new GroundsModel(CalculateGroundsLenght());


        private int CalculateGroundsLenght() => 100;
    }
}
