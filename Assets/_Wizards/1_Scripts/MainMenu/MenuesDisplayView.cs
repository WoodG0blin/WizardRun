using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class MenuesDisplayView : MonoBehaviour
    {
        [SerializeField] private WorldPanelView _locations;
        [SerializeField] private RanksView _rankings;
        [SerializeField] private InventoryView _inventory;
        [SerializeField] private ShopView _shop;
        [SerializeField] private SettingsView _settings;
        //[Space(10)]
        //[SerializeField] private Button _locationsButton;
        //[SerializeField] private Button _ranksButton;
        //[SerializeField] private Button _inventoryButton;
        //[SerializeField] private Button _shopButton;
        //[SerializeField] private Button _settingsButton;

        //private List<MenuPanelView> _menues;
        private MenuPanelsManager _menuesManager;

        public void Init(IMenuInfo menuInfo, Action<Location> onStart)
        {
            _menuesManager = new(
                panels: new() { _locations, _rankings, _inventory, _shop, _settings },
                input: menuInfo);

            //_locationsButton.onClick.AddListener(ShowLocations);
            //_ranksButton.onClick.AddListener(ShowRanks);
            //_inventoryButton.onClick.AddListener(ShowInventory);
            //_shopButton.onClick.AddListener(ShowShop);
            //_settingsButton.onClick.AddListener(ShowSettings);

            //for (int i = 0; i < _menues.Count; i++)
            //    _menues[i].Init(menuInfo);
            _locations.OnStart = onStart;
        }

        public void ShowLocations() => _menuesManager.ActivatePanel(_locations);

        //public void ShowLocations() => ActivateMenuByIndex(_menues.IndexOf(_locations));
        //private void ShowRanks() => ActivateMenuByIndex(_menues.IndexOf(_rankings));
        //private void ShowInventory() => ActivateMenuByIndex(_menues.IndexOf(_inventory));
        //private void ShowShop() => ActivateMenuByIndex(_menues.IndexOf(_shop));
        //private void ShowSettings() => ActivateMenuByIndex(_menues.IndexOf(_settings));

        //private void ActivateMenuByIndex(int index)
        //{
        //    for (int i = 0; i < _menues.Count; i++)
        //        _menues[i].SetActive(i == index);
        //}
    }

}
