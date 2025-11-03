using System;
using UnityEngine;

namespace WizardsPlatformer
{
    public class SoundSettingsView : MonoBehaviour
    {
        [SerializeField] private StatSliderView _musicVolumeSlider;
        [SerializeField] private StatSliderView _sfxVolumeSlider;
        [SerializeField] private ButtonView _muteButton;

        [SerializeField] private Sprite _muteOn;
        [SerializeField] private Sprite _muteOff;

        private SoundSettings _soundSettings;

        private Action<SoundSettings> _updateSettings;

        public void Init(Action<SoundSettings> updateSettings)
        {
            _updateSettings = updateSettings;
        }

        public void Activate(SoundSettings current)
        {
            _soundSettings = current;

            _musicVolumeSlider.SetValue(_soundSettings.MusicVolume);
            _sfxVolumeSlider.SetValue(_soundSettings.SFXVolume);
            _muteButton.SetImage(_soundSettings.IsMuted ? _muteOn : _muteOff);

            _musicVolumeSlider.OnValueChanged = UpdateMusic;
            _sfxVolumeSlider.OnValueChanged = UpdateEffects;

            _muteButton.SetClick(ToggleMute);
        }

        private void UpdateMusic(int value)
        {
            _soundSettings.MusicVolume = value;
            _updateSettings?.Invoke(_soundSettings);
        }
        private void UpdateEffects(int value)
        {
            _soundSettings.SFXVolume = value;
            _updateSettings?.Invoke(_soundSettings);
        }

        private void ToggleMute()
        {
            _soundSettings.IsMuted = !_soundSettings.IsMuted;
            _muteButton.SetImage(_soundSettings.IsMuted ? _muteOn : _muteOff);
            _updateSettings?.Invoke(_soundSettings);
        }
    }
}
