using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class MenuesDisplayView : MonoBehaviour
    {
        [SerializeField] private AccountInfoDisplayView _accountDisplay;
        [SerializeField] private InventoryView _inventory;
        [SerializeField] private ShopView _shop;
        [SerializeField] private SettingsView _settings;
        [Space(10)]
        [SerializeField] private Button _closeMenuesButton;
        [SerializeField] private GameObject _menuesDisplayPanel;
        [Space(10)]
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _settingsButton;

        private List<MenuPanelView> _menues;

        public Action OnInfoClick;

        private void Awake()
        {
            _closeMenuesButton.onClick.AddListener(CloseAll);
            _menues = new() { _accountDisplay, _inventory, _shop, _settings };

            _infoButton.onClick.AddListener(ShowInfo);
            _inventoryButton.onClick.AddListener(ShowInventory);
            _shopButton.onClick.AddListener(ShowShop);
            _settingsButton.onClick.AddListener(ShowSettings);

            SetActiveDisplay(false);
        }

        public void Init(IMenuInfo menuInfo)
        {
            for (int i = 0; i < _menues.Count; i++)
                _menues[i].Init(menuInfo);
            
            ActivateMenuByIndex(_menues.IndexOf(_accountDisplay));

            //SetControls
        }

        private void ShowInfo() => ActivateMenuByIndex(_menues.IndexOf(_accountDisplay));
        private void ShowInventory() => ActivateMenuByIndex(_menues.IndexOf(_inventory));
        private void ShowShop() => ActivateMenuByIndex(_menues.IndexOf(_shop));
        private void ShowSettings() => ActivateMenuByIndex(_menues.IndexOf(_settings));

        private void ActivateMenuByIndex(int index)
        {
            SetActiveDisplay(true);
            _closeMenuesButton.gameObject.SetActive(true);

            for (int i = 0; i < _menues.Count; i++)
                _menues[i].SetActive(i == index);
        }

        private void CloseAll()
        {
            ActivateMenuByIndex(-1);
            SetActiveDisplay(false);
            _closeMenuesButton.gameObject.SetActive(false);
        }

        private void SetActiveDisplay(bool active) => _menuesDisplayPanel.SetActive(active);
    }
}
