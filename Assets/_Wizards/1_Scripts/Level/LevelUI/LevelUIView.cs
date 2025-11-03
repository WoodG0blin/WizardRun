using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelUIView : MonoBehaviour
    {
        [SerializeField] private LevelDisplayView _levelDisplayView;
        [SerializeField] private InputView _inputView;
        [SerializeField] private PauseDisplayView _pauseDisplay;
        [SerializeField] private FinishDisplayView _finishDisplay;

        public IInputView Input => _inputView;
        public IPlayerDisplay PlayerDisplay => _levelDisplayView;

        public Action OnRestartRequest;
        public Action OnRunRequest;
        private Action _onFinish;

        public void Init(ILevelInfo info)
        {
            _levelDisplayView.InitPlayer(info.PlayerModel);
            _levelDisplayView.SetLevelClearanceValue(0f);

            _onFinish = info.SceneLoader.LoadMainMenu;

            _pauseDisplay.Init(
                onRun: () => OnRunRequest?.Invoke(),
                sound: info.SoundManager);
            _finishDisplay.Init(
                onExit: () => _onFinish?.Invoke(),
                onRestart: () => OnRestartRequest?.Invoke());

            _inputView.OnPauseMenu = _pauseDisplay.Display;
        }


        public void DisplayFinish(bool isWin, float score, Dictionary<BonusType, int> bonuses) =>
            _finishDisplay.Display(isWin, Mathf.RoundToInt(score * 100), bonuses);
    }
}
