using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace WizardsPlatformer
{
    internal class InventoryView : MenuPanelView
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private TextMeshProUGUI _itemInfoText;

        [SerializeField] private EquipDisplayView _equipDisplay;

        protected override void OnInit()
        {
            _equipDisplay.Init();
            Display();
        }

        private void Display()
        {
            Clear();

            _equipDisplay.Display(menuInfo.PlayerModel.EquippedArtifacts, RemoveItem);

            List<IArtifact> unequippedItems = new();
            foreach(var i in menuInfo.ArtifactDatabase)
            {
                var check = menuInfo.PlayerModel.EquippedArtifacts.Where(a => a.Name == i.Name).ToList();
                if(check == null || check.Count == 0 )
                    unequippedItems.Add(CreateFromConfig(i));
            }

            foreach (var item in unequippedItems)
                DisplayItem(item);
        }

        private void Clear()
        {
            for (int i = _container.childCount - 1; i > 0; i--)
                GameObject.Destroy(_container.GetChild(i).gameObject);
        }

        private void DisplayItem(IArtifact item)
        {
            GameObject.Instantiate(_itemPrefab, _container)
                .GetComponent<InventoryItemView>()
                .Init(item, () => SetItem(item));
        }

        private void SetItem(IArtifact item)
        {
            DisplayItemInfo(item.Name);

            if(menuInfo.PlayerModel.TryEquipArtifact(item))
                Display();
        }

        private void RemoveItem(IArtifact item)
        {
            menuInfo.PlayerModel.RemoveArtifact(item);
            Display();
        }

        private void DisplayItemInfo(string info) =>
            _itemInfoText.text = info;

        private IArtifact CreateFromConfig(ItemConfig config) => new Artifact(config);
    }
}
