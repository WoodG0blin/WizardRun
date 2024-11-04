using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;

namespace WizardsPlatformer
{
    internal class LevelDisplayView : View
    {
        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] private TextMeshProUGUI _coins;

        public void SetHealth(int health) => _health.text = $"{health}";
        public void SetCoinsCount(int coins) => _coins.text = $"{coins}";
    }
}