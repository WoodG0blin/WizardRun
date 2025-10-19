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
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _container;

        [SerializeField] private TextMeshProUGUI _itemInfoText;

        [SerializeField] private EquipDisplayView _equipDisplay;

        public Transform Container;

        protected override void OnInit()
        {
            Display();
        }


        private void Display()
        {
            Clear();

            List<InventoryItemView> unequippedItems = new();
            foreach (var i in menuInfo.PlayerModel.Chest)
            {
                var item = CreateItem(i);
                unequippedItems.Add(item);
                DisplayItem(item);
            }

            List<InventoryItemView> equippedItems = new();
            foreach (var a in menuInfo.PlayerModel.EquippedArtifacts)
            {
                var item = CreateItem(a.Config);
                equippedItems.Add(item);
            }
            _equipDisplay.Display(equippedItems);
        }

        private void Clear()
        {
            for (int i = _container.childCount - 1; i >= 0; i--)
                GameObject.Destroy(_container.GetChild(i).gameObject);
        }
        private InventoryItemView CreateItem(ItemConfig artifact)
        {
            var temp = GameObject.Instantiate(_itemPrefab).GetComponent<InventoryItemView>();
            temp.Init(artifact, () => DisplayItemInfo(artifact), Container);
            return temp;
        }
        private void DisplayItemInfo(IItem item)
        {
            _itemInfoText.text = item.Name;
            _equipDisplay.HighlightSlot(item.SlotType);
        }

        private void DisplayItem(InventoryItemView item)
        {
            var temp = GameObject.Instantiate(_slotPrefab, _container)
                .GetComponent<InventorySlotView>();
            temp.Init();
            temp.SetItem(item);
        }
    }
}
