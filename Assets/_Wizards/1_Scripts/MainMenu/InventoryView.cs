using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class InventoryView : MenuPanelView
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private TextMeshProUGUI _itemInfoText;

        private List<ItemConfig> _selectedItems;

        protected override void OnInit()
        {
            Display(menuInfo.ArtifactDatabase);
        }

        private void Display(IEnumerable<ItemConfig> items)
        {
            Clear();
            _selectedItems = new();

            foreach (var item in items)
                DisplayItem(item);
        }

        private void Clear()
        {
            for (int i = _container.childCount - 1; i > 0; i--)
                GameObject.Destroy(_container.GetChild(i).gameObject);
        }

        private void DisplayItem(ItemConfig item)
        {
            GameObject.Instantiate(_itemPrefab, _container)
                .GetComponent<InventoryItemView>()
                .Init(item, selected => SetItem(item, selected));
        }

        private void SetItem(ItemConfig item, bool selected)
        {
            if (selected) _selectedItems.Add(item);
            else _selectedItems.Remove(item);

            DisplayItemInfo(item, selected);

            menuInfo.EquipArtifacts(GenerateSelected());
        }

        private List<Artifact> GenerateSelected()
        {
            List<Artifact> res = new();

            foreach (var a in _selectedItems)
                if(a != null) res.Add(new Artifact(a));

            return res;
        }

        private void DisplayItemInfo(ItemConfig item, bool selected) =>
            _itemInfoText.text = selected ? item.NameTag : "";
    }
}
