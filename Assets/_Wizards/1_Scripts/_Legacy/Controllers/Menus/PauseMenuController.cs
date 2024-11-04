using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal enum LevelResult { Pause, Success, Failure }

    internal class PauseMenuController : Controller
    {
        private readonly Transform _UIContainer;
        private readonly string _assetPath = "UI/PauseMenu"; //TODO: replace with SO config
        private PauseMenuView _pauseMenuView;
        private Action<PauseMenuResult> _onPauseMenuResult;

        public PauseMenuController(Transform UIContainer, Action<PauseMenuResult> onPauseMenuResult)
        {
            _UIContainer = UIContainer;
            _onPauseMenuResult = onPauseMenuResult;

            GameObject temp = GameObject.Instantiate(ResourceLoader.LoadPrefab(_assetPath), _UIContainer);
            _pauseMenuView = temp.GetComponent<PauseMenuView>() ?? temp.AddComponent<PauseMenuView>();

            _pauseMenuView.OnPauseMenuFinished = ProcessPauseMenuResult;
            _pauseMenuView.Deactivate();

            //_pauseMenuView.OnResume = () => { };
            //_pauseMenuView.OnRestart = () => { _GameModel.CurrentState.Value = GameState.Game; };
            //_pauseMenuView.OnMainMenu = () => { _GameModel.CurrentState.Value = GameState.MainMenu; };
            //_pauseMenuView.OnExit = () => { _GameModel.CurrentState.Value = GameState.Exit; };

            Register(_pauseMenuView);
        }

        public void Activate(LevelResult result, BonusStats bonuses)
        {
            _pauseMenuView.Activate(result, bonuses);
        }

        private void ProcessPauseMenuResult(PauseMenuResult result)
        {
            _pauseMenuView.Deactivate();
            _onPauseMenuResult(result);
        }
    }
}
