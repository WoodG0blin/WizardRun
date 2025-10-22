using UnityEngine;
using System;
using System.Collections.Generic;

namespace WizardsPlatformer
{
    public class FinishDisplayView : MonoBehaviour
    {
        [SerializeField] private ButtonView _restartButton;
        [SerializeField] private ButtonView _mainMenuButton;

        [SerializeField] private GameObject _winLabel;
        [SerializeField] private GameObject _failLabel;

        [SerializeField] private TMPro.TextMeshProUGUI _scoreText;
        [SerializeField] private StatDisplayView _coinsDisplay;
        [SerializeField] private StatDisplayView _soulsDisplay;
        [SerializeField] private StatDisplayView _artifactsDisplay;

        private Action _onMainMenu;
        private Action _onRestart;

        public void Init(Action onExit, Action onRestart)
        {
            _onMainMenu = onExit;
            _onRestart = onRestart;

            _restartButton.SetClick(RestartClick);
            _mainMenuButton.SetClick(MainMenuClick);
        }


        public void Display(bool isWin, int score, Dictionary<BonusType, int> bonuses)
        {
            gameObject.SetActive(true);

            _winLabel.SetActive(isWin);
            _failLabel.SetActive(!isWin);

            _scoreText.text = $"LEVEL MASTERED: {score}%";

            _coinsDisplay.SetValue(bonuses[BonusType.Coin]);
            _soulsDisplay.SetValue(bonuses[BonusType.Souls]);
            _artifactsDisplay.SetValue(bonuses[BonusType.Artifacts]);

            _restartButton.gameObject.SetActive(!isWin);
        }

        private void RestartClick()
        {
            gameObject.SetActive(false);
            _onRestart?.Invoke();
        }

        private void MainMenuClick()
        {
            gameObject.SetActive(false);
            _onMainMenu?.Invoke();
        }
    }
}
