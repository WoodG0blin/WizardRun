using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class InventoryView : MenuPanelView
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _container;

        [SerializeField] private TextMeshProUGUI _itemInfoText;

        [SerializeField] private EquipDisplayView _equipDisplay;

        private Dictionary<InventoryItemView, IArtifact> _inventoryItems;

        protected override void OnInit()
        {
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
                var check = menuInfo.PlayerModel.EquippedArtifacts.Where(a => a.Name == i.Name).ToList();
                if (check == null || check.Count == 0)
                    unequippedItems.Add(CreateItem(CreateFromConfig(i)));
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
            return menuInfo.PlayerModel.TrySetArtifactAt(slot, art);
            //Display();
        }

        private void DisplayItemInfo(IArtifact item)
        {
            _itemInfoText.text = item.Name;
            _equipDisplay.HighlightSlot(item.SlotType);
        }

        private IArtifact CreateFromConfig(ItemConfig config) => new Artifact(config);

        private InventoryItemView CreateItem(IArtifact artifact)
        {
            var temp = GameObject.Instantiate(_itemPrefab).GetComponent<InventoryItemView>();
            temp.Init(artifact.Icon, artifact.SlotType, () => DisplayItemInfo(artifact));
            _inventoryItems.Add(temp, artifact);
            return temp;
        }
    }
}
