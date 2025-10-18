using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class StatsUpgradePanelView : MenuPanelView
    {
        [SerializeField] private TMPro.TextMeshProUGUI _availableChangeValueText;
        [SerializeField] private List<StatSliderView> _sliders;

        protected override void OnInit()
        {
            foreach (var slider in _sliders) slider.OnValueChanged += v => ChangeModifier(slider.StatType, v);
        }

        protected override void OnActivation()
        {
            var playerStats = menuInfo.PlayerModel.Stats;

            _availableChangeValueText.text = $"{playerStats.Modifiers.AvailableModsCount} (remains {playerStats.Modifiers.UnusedModsCount})";

            foreach (var slider in _sliders)
                slider.SetValue(playerStats.Modifiers.GetBaseModifier(slider.StatType));

            bool active = GetTotalSlidersValue() < playerStats.Modifiers.AvailableModsCount;
            foreach(var slider in _sliders)
                slider.SetActiveIncrease(active);
        }

        private void ChangeModifier(CharacterStatType stat, int value)
        {
            var playerStats = menuInfo.PlayerModel.Stats;
            playerStats.Modifiers.SetBaseModifier(stat, value);
            Debug.Log($"Requesting base modifier for {stat} by {value}");
            OnActivation();
        }

        private int GetTotalSlidersValue()
        {
            int res = 0;
            foreach (var slider in _sliders) res += slider.Value;
            return res;
        }
    }
}
