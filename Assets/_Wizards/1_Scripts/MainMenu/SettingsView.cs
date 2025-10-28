using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class SettingsView : MenuPanelView
    {
        [SerializeField] private StatSliderView _musicVolumeSlider;
        [SerializeField] private StatSliderView _sfxVolumeSlider;
        [SerializeField] private ButtonView _muteButton;

        [SerializeField] private Sprite _muteOn;
        [SerializeField] private Sprite _muteOff;

        private SoundSettings _soundSettings;

        public Action OnSettingsSet { get; set; }

        protected override void OnInit()
        {
            _soundSettings = menuInfo.SoundManager.SoundSettings;
        }

        protected override void OnActivation()
        {
            _activationButton.SetClick(ResetActivation);

            _musicVolumeSlider.SetValue(_soundSettings.MusicVolume);
            _sfxVolumeSlider.SetValue(_soundSettings.SFXVolume);
            _muteButton.SetImage(_soundSettings.IsMuted ? _muteOn : _muteOff);

            _musicVolumeSlider.OnValueChanged = v => _soundSettings.MusicVolume = v;
            _sfxVolumeSlider.OnValueChanged = v => _soundSettings.SFXVolume = v;

            _muteButton.SetClick(ToggleMute);
        }

        private void ResetActivation()
        {
            _activationButton.SetClick(() => SetActive(true));
            OnSettingsSet?.Invoke();
        }

        private void ToggleMute()
        {
            _soundSettings.IsMuted = !_soundSettings.IsMuted;
            _muteButton.SetImage(_soundSettings.IsMuted ? _muteOn : _muteOff);
        }

        private void OnDisable()
        {
            menuInfo.SoundManager.UpdateSettings(_soundSettings);
        }
    }

    [Serializable]
    public class SoundSettings
    {
        public int MusicVolume = 2;
        public int SFXVolume = 2;
        public bool IsMuted = false;
    }
}
