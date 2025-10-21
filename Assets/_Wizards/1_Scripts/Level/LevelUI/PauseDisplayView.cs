using UnityEngine;
using System;

namespace WizardsPlatformer
{
    public class PauseDisplayView : MonoBehaviour
    {
        [SerializeField] private ButtonView _pauseButton;
        [SerializeField] private ButtonView _mainMenuButton;
        [SerializeField] private ButtonView _resumeButton;

        private Action _onMainMenu;
        private Action<bool> _onPaused;

        public void Init(Action onRun)
        {
            _onMainMenu = onRun;

            _pauseButton.SetClick(Pause);
            _resumeButton.SetClick(Resume);
            _mainMenuButton.SetClick(MainMenu);
        }

        public void Display() => _pauseButton.Trigger();

        private void Pause()
        {
            gameObject.SetActive(true);
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
            _onMainMenu?.Invoke();
        }
    }
}
