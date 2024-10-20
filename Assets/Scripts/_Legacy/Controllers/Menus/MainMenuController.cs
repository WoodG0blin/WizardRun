using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class MainMenuController : Controller
    {
        private readonly Transform _UIContainer;
        private readonly GameModel _GameModel;
        private readonly string _assetPath = "UI/MainMenu"; //TODO: replace with SO config

        public MainMenuController(Transform uIContainer, GameModel gameModel)
        {
            _UIContainer = uIContainer;
            _GameModel = gameModel;

            GameObject temp = GameObject.Instantiate(ResourceLoader.LoadPrefab(_assetPath), _UIContainer);
            MainMenuView _menuView = temp.GetComponent<MainMenuView>() ?? temp.AddComponent<MainMenuView>();
            _menuView.OnStart = OnGameStart;
            _menuView.OnSettings = OnSettings;
            _menuView.OnInventory = () => { _GameModel.CurrentState.Value = GameState.Inventory; };
            _menuView.OnExit = () => { _GameModel.CurrentState.Value = GameState.Exit; };
            _menuView.OnShop = () => { _GameModel.CurrentState.Value = GameState.Shop; };
            _menuView.SetCoinsCount(_GameModel.Bonuses[BonusType.coin]);
            Register(_menuView);

            _GameModel.AnalyticsManager.OnMenuEnter();

            //if (_GameModel.AdsManager.IsInitialized) _GameModel.AdsManager.InterstitialPlayer.Play();
            //else _GameModel.AdsManager.OnInitialized.AddListener(_GameModel.AdsManager.InterstitialPlayer.Play);
        }

        private void OnGameStart() => _GameModel.CurrentState.Value = GameState.Game;
        private void OnSettings() => _GameModel.CurrentState.Value = GameState.Settings;
    }
}
