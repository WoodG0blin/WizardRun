using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class InventoryView : MenuPanelView
    {
        [SerializeField] private MenuPanelView _equip;
        [SerializeField] private MenuPanelView _craft;

        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _container;

        [SerializeField] private TextMeshProUGUI _itemInfoText;

        [SerializeField] private EquipDisplayView _equipDisplay;

        private Dictionary<InventoryItemView, IItem> _inventoryItems;
        private MenuPanelsManager _subPanelsManager;

        protected override void OnInit()
        {
            _subPanelsManager = new(
                panels: new() { _equip, _craft },
                input: menuInfo);

            _subPanelsManager.ActivatePanel(_equip);

            _inventoryItems = new();
            _equipDisplay.Init(transform, TryEquipNewItemTo);
            Display();
        }


        private void Display()
        {
            Clear();

            List<InventoryItemView> equippedItems = new();
            foreach (var a in menuInfo.PlayerModel.EquippedArtifacts)
                equippedItems.Add(CreateItem(a));

            _equipDisplay.Display(equippedItems);

            List<InventoryItemView> unequippedItems = new();
            foreach (var i in menuInfo.ArtifactDatabase)
            {
                var check = menuInfo.PlayerModel.EquippedArtifacts.Where(a => a.NameTag == i.NameTag).ToList();
                if (check == null || check.Count == 0)
                    unequippedItems.Add(CreateItem(i));
            }

            foreach (var item in unequippedItems)
                DisplayItem(item);
        }

        private void Clear()
        {
            for (int i = _container.childCount - 1; i >= 0; i--)
                GameObject.Destroy(_container.GetChild(i).gameObject);
        }

        private void DisplayItem(InventoryItemView item)
        {
            var temp = GameObject.Instantiate(_slotPrefab, _container)
                .GetComponent<InventorySlotView>();
            temp.Init(transform);
            temp.TrySetItem(item);
        }

        private bool TryEquipNewItemTo(InventoryItemView item, ArtifactSlotType slot)
        {
            var art = item != null ? _inventoryItems[item] : null;
            return menuInfo.PlayerModel.TrySetArtifactAt(
                slot,
                menuInfo.ArtifactDatabase.Where(c => c.NameTag == art.NameTag).FirstOrDefault());
            //Display();
        }

        private void DisplayItemInfo(IItem item)
        {
            _itemInfoText.text = item.Name;
            _equipDisplay.HighlightSlot(item.SlotType);
        }


        private InventoryItemView CreateItem(IItem artifact)
        {
            var temp = GameObject.Instantiate(_itemPrefab).GetComponent<InventoryItemView>();
            temp.Init(artifact.Icon, artifact.SlotType, () => DisplayItemInfo(artifact));
            _inventoryItems.Add(temp, artifact);
            return temp;
        }
    }
}
