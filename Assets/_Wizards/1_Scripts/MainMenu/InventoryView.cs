using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace WizardsPlatformer
{
    internal class InventoryView : MenuPanelView, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _container;

        [SerializeField] private TextMeshProUGUI _itemInfoText;

        [SerializeField] private EquipDisplayView _equipDisplay;

        public Transform Container;

        private List<InventorySlotView> _chestSlots;

        protected override void OnInit()
        {
            Display();
        }


        private void Display()
        {
            Clear();

            _chestSlots = new();
            for(int i = 0; i < menuInfo.PlayerModel.MaxInventorySlots; i++)
            {
                var temp = GameObject.Instantiate(_slotPrefab, _container).GetComponent<InventorySlotView>();
                temp.Init();
                _chestSlots.Add(temp);
            }

            List<InventoryItemView> unequippedItems = new();
            foreach (var i in menuInfo.PlayerModel.Chest)
            {
                if (i != null)
                {
                    var item = CreateItem(i);
                    unequippedItems.Add(item);
                    DisplayItem(item);
                }
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
            temp.Init(artifact, DisplayItemInfo, Container);
            return temp;
        }
        private void DisplayItemInfo(ItemConfig item)
        {
            if (item != null)
            {
                _itemInfoText.text = item.Name;
                _equipDisplay.HighlightSlot(item.SlotType);
            }
            else
            {
                _itemInfoText.text = "";
                _equipDisplay.HighlightSlot(ArtifactSlotType.Universal);
            }
        }

        private void DisplayItem(InventoryItemView item)
        {
            for (int i = 0; i < _chestSlots.Count; i++)
            {
                if (_chestSlots[i].Item == null)
                {
                    _chestSlots[i].SetItem(item);
                    break;
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            InventoryItemView initialItem = eventData.pointerDrag.GetComponent<InventoryItemView>();
            if (initialItem == null) return;

            InventorySlotView startSlot = initialItem.ParentSlot;

            bool hasEmpty = false;
            for (int i = 0; i < _chestSlots.Count; i++)
            {
                hasEmpty = _chestSlots[i].Item == null;
                if (hasEmpty) break;
            }

            if (hasEmpty)
            {
                startSlot.SetItem(null);
                DisplayItem(initialItem);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _itemInfoText.text = "";
            _equipDisplay.HighlightSlot(ArtifactSlotType.Universal);
        }
    }
}
