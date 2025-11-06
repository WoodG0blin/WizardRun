using UnityEngine;
using System;

namespace WizardsPlatformer
{
    public class PauseDisplayView : MonoBehaviour
    {
        [SerializeField] private ButtonView _pauseButton;
        [SerializeField] private ButtonView _mainMenuButton;
        [SerializeField] private ButtonView _resumeButton;
        [SerializeField] private SoundSettingsView _soundSettings;

        private Action _onMainMenu;
        private Action<bool> _onPaused;

        private SoundManager soundManager;

        public void Init(Action onRun, SoundManager sound)
        {
            _onMainMenu = onRun;

            _pauseButton.SetClick(Pause);
            _resumeButton.SetClick(Resume);
            _mainMenuButton.SetClick(MainMenu);

            soundManager = sound;
            _soundSettings.Init(soundManager.UpdateSettings);
        }

        public void Display() => _pauseButton.Trigger();

        private void Pause()
        {
            gameObject.SetActive(true);
            _soundSettings.Activate(soundManager.SoundSettings);
            Time.timeScale = 0;
            _onPaused?.Invoke(true);
        }

        private void Resume()
        {
            gameObject?.SetActive(false);
            Time.timeScale = 1f;
            _onPaused?.Invoke(false);
        }

        private void MainMenu()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            _onMainMenu?.Invoke();
        }
    }
}
