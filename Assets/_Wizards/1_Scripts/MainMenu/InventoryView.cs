using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class InventoryView : MenuPanelView
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private Button _applyButton;

        private List<ItemConfig> _selectedItems;
        private Action<List<Artifact>> _onApplySelection;

        protected override void OnInit()
        {
            _applyButton.onClick.AddListener(ApplySelectedArtifacts);
        }

        public void Display(IEnumerable<ItemConfig> items)
        {
            gameObject.SetActive(true);

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
                .GetComponent<ItemView>()
                .Init(item, selected => SetItem(item, selected));
        }

        private void SetItem(ItemConfig item, bool selected)
        {
            if (selected) _selectedItems.Add(item);
            else _selectedItems.Remove(item);
        }

        private void ApplySelectedArtifacts()
        {
            menuInfo.EquipArtifacts(GenerateSelected());
        }

        private List<Artifact> GenerateSelected()
        {
            List<Artifact> res = new();

            foreach (var a in _selectedItems)
            {
                res.Add(new Artifact(a));
                //ArtifactFactory.GetArtifact(a);
                //if (a is WeaponConfig w) res.Add(new Weapon(w));
                //else res.Add(new Artifact(a));
            }

            return res;
        }
    }
}
