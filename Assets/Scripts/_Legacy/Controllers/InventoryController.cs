using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WizardsPlatformer
{
    internal class InventoryController : Controller
    {
        private readonly GameModel _gameModel;

        private readonly InventoryModel _inventoryModel;
        private readonly IInventoryView _inventoryView;
        private readonly ItemsRepository _itemsRepository;

        private readonly string _assetPath = "UI/Inventory";
        private readonly string _DataSourcePath = "ItemConfigs";

        public InventoryController(Transform UIContainer, GameModel gameModel)
        {
            _gameModel = gameModel;
            _inventoryModel = _gameModel.InventoryModel;
            _inventoryView = GameObject.Instantiate(ResourceLoader.Load<GameObject>(_assetPath)).GetComponent<InventoryView>();
            _inventoryView.OnReturn = () => { _gameModel.CurrentState.Value = GameState.MainMenu; };
            _inventoryView.OnApply = () => ApplyChangesToPlayer();
            Register(_inventoryView);
            _itemsRepository = new ItemsRepository(ResourceLoader.LoadFromDataSource<AllItemConfigs, ItemConfig>(_DataSourcePath));
            Register(_itemsRepository);

            _inventoryView.Display(_itemsRepository.Items.Values, OnItemClicked);

            foreach(string item in _inventoryModel.EquippedItems) _inventoryView.Select(item, true);
        }

        private void ApplyChangesToPlayer()
        {
            _gameModel.LevelModel.PlayerModel.Reset();

            foreach (string item in _inventoryModel.EquippedItems)
            {
                var temp = _itemsRepository.Items[item];
                if (temp.IsUpgrade) _gameModel.LevelModel.PlayerModel.AddUpgrade(temp.Upgrade);
            }

            _gameModel.CurrentState.Value = GameState.MainMenu;
        }

        private void OnItemClicked(string ItemID)
        {
            if(_inventoryModel.IsEquipped(ItemID)) _inventoryModel.UnEquipItem(ItemID);
            else _inventoryModel.EquipItem(ItemID);

            _inventoryView.Select(ItemID, _inventoryModel.IsEquipped(ItemID));
        }
    }
}