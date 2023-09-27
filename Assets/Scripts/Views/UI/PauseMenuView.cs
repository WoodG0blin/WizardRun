using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal enum PauseMenuResult { Resume, Restart, StartNew, MainMenu }
    internal interface IPauseMenuView
    {
        Action<PauseMenuResult> OnPauseMenuFinished { set; }
        void Activate(LevelResult levelResult, BonusStats bonuses);
        void Deactivate();
    }

    internal class PauseMenuView : View, IPauseMenuView
    {
        [SerializeField] private Image _successImage;
        [SerializeField] private Image _failureImage;
        [SerializeField] private Image _pauseImage;

        [SerializeField] private GameObject _successField;
        [SerializeField] private GameObject _failureField;
        [SerializeField] private GameObject _pauseField;

        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _startNewButton;
        [SerializeField] private Button _mainMenuButton;

        [SerializeField] private TextMeshProUGUI _coinsValue;

        private Image _currentImage;
        private GameObject _currentField;

        public Action<PauseMenuResult> OnPauseMenuFinished
        {
            set
            {
                _resumeButton.onClick.AddListener(() => value.Invoke(PauseMenuResult.Resume));
                _restartButton.onClick.AddListener(() => value.Invoke(PauseMenuResult.Restart));
                _startNewButton.onClick.AddListener(() => value.Invoke(PauseMenuResult.StartNew));
                _mainMenuButton.onClick.AddListener(() => value.Invoke(PauseMenuResult.MainMenu));
            }
        }

        public void Activate(LevelResult levelResult, BonusStats bonuses)
        {
            gameObject.SetActive(true);

            _currentField?.SetActive(false);
            if(_currentImage!= null) _currentImage.gameObject.SetActive(false);

            (_currentImage, _currentField) = levelResult switch
            {
                LevelResult.Pause => (_pauseImage, _pauseField),
                LevelResult.Success => (_successImage, _successField),
                LevelResult.Failure => (_failureImage, _failureField),
                _ => (null, null)
            };

            if (levelResult == LevelResult.Success) _coinsValue.text = $"{bonuses[BonusType.coin]} coins collected";

            _currentField?.SetActive(true);
            if (_currentImage != null) _currentImage.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }

        //public UnityAction OnResume { set => _resumeButton.onClick.AddListener(value); }
        //public UnityAction OnRestart { set => _restartButton.onClick.AddListener(value); }
        //public UnityAction OnMainMenu { set => _mainMenuButton.onClick.AddListener(value); }
        //public UnityAction OnExit { set  { } }
    }
}
