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
        internal AnalyticsManager AnalyticsManager { get; }
        //internal AdsManager AdsManager { get; }

        public GameModel(GameState initialState)
        {
            CurrentState = new SubscribrablePropertyWithEqualsCheck <GameState>();
            CurrentState.Value = initialState;
            InventoryModel = new InventoryModel();
            LevelModel = new LevelModel(CurrentState, AddBonus);
            AnalyticsManager = GameObject.FindObjectOfType<AnalyticsManager>();
            //AdsManager= GameObject.FindObjectOfType<AdsManager>();
            Bonuses = new BonusStats(true);
        }

        public void AddBonus(BonusType type, int value) => Bonuses[type] += value;
    }
}
