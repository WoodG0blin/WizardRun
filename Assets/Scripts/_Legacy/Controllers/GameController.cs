using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class GameController : Controller
    {
        private readonly Transform _UIContainer;
        private readonly GameModel _gameModel;

        private Controller _activeController;

        private LevelConfig _levelConfig;
        private string _configPath = "LevelConfig";

        public GameController(Transform UIContainer, GameModel gameModel)
        {
            _UIContainer = UIContainer;
            _gameModel = gameModel;
            _levelConfig = ResourceLoader.Load<LevelConfig>(_configPath);

            _gameModel.CurrentState.SubscribeOnValueChange(OnChangeGameState);
            OnChangeGameState(_gameModel.CurrentState.Value);
        }

        private void OnChangeGameState(GameState newState)
        {
            _activeController?.Dispose();

            switch(newState)
            {
                case GameState.Game: _activeController = new LevelController(_levelConfig, _gameModel, _UIContainer); break;
                case GameState.MainMenu: _activeController = new MainMenuController(_UIContainer, _gameModel); break;
                case GameState.Settings: _activeController = new SettingsController(_UIContainer, _gameModel); break;
                case GameState.Inventory: _activeController = new InventoryController(_UIContainer, _gameModel); break;
                case GameState.Shop: _activeController = new ShopController(_UIContainer, _gameModel); break;
                case GameState.Exit: Application.Quit(); break;
                default: break;
            }
        }

        protected override void OnDispose()
        {
            _gameModel.CurrentState.UnsubscribeOnValueChange(OnChangeGameState);
        }
    }
}
