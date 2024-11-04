using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

namespace WizardsPlatformer
{
    internal interface IMainMenuView
    {
        Action OnExit { set; }
        Action OnInventory { set; }
        Action OnSettings { set; }
        Action OnShop { set; }
        Action OnStart { set; }
    }

    internal class MainMenuView : View, IMainMenuView
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TextMeshProUGUI _coinsText;

        public Action OnStart { set => ButtonTweenInit.SetAction(_startButton, value); }
        public Action OnSettings { set => ButtonTweenInit.SetAction(_settingsButton, value); }
        public Action OnInventory { set => ButtonTweenInit.SetAction(_inventoryButton, value); }
        public Action OnShop { set => ButtonTweenInit.SetAction(_shopButton, value); }
        public Action OnExit { set => ButtonTweenInit.SetAction(_exitButton, value); }
        public void SetCoinsCount(int value) => _coinsText.text = value.ToString();
    }
}
