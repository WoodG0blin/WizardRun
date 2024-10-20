using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace WizardsPlatformer
{
    internal class LevelDisplayController : Controller
    {
        private LevelDisplayView _display;

        public LevelDisplayController(MenuConfig config, Transform UIContainer)
        {
            GameObject temp = GameObject.Instantiate(config.Prefab);
            _display = temp.GetComponent<LevelDisplayView>() ?? temp.AddComponent<LevelDisplayView>();
        }

        public void OnHealthChange(float health) => _display.SetHealth(health);
        public void OnBonusChange(BonusType type, int coins)
        {
            if (type == BonusType.coin) _display.SetCoinsCount(coins);
        }

        public void SetActive(bool active) => _display.SetActive(active);
        protected override void OnDispose()
        {
            GameObject.Destroy(_display.gameObject);
        }
    }
}