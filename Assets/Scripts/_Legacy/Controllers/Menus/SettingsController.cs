using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class SettingsController : Controller
    {
        private readonly Transform _UIContainer;
        private readonly GameModel _GameModel;
        private readonly string _assetPath = "UI/Settings"; //TODO: replace with SO config

        public SettingsController(Transform UIContainer, GameModel gameModel)
        {
            _UIContainer = UIContainer;
            _GameModel = gameModel;

            GameObject temp = GameObject.Instantiate(ResourceLoader.LoadPrefab(_assetPath), _UIContainer);
            SettingsView _settingsView = temp.GetComponent<SettingsView>() ?? temp.AddComponent<SettingsView>();
            _settingsView.OnReturn = OnReturn;
            Register(_settingsView);
        }

        private void OnReturn() => _GameModel.CurrentState.Value = GameState.MainMenu;
    }
}
