using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class InventorySlotView : MonoBehaviour, IDropHandler
    {
        public ArtifactSlotType SlotType = ArtifactSlotType.Universal;
        public InventoryItemView Item { get; private set; }

        [Space(10)]
        [SerializeField] private Image _background;

        [SerializeField] private Color _selected;
        [SerializeField] private Color _unselected;
        [field: SerializeField] public RectTransform ItemPlace { get; private set; }

        public Action<InventoryItemView, InventoryItemView> OnNewItemPlaced { get; set; }

        public void Init()
        {
            gameObject.SetActive(true);

            Highlight(false);

            if(Item!= null) GameObject.Destroy(Item.gameObject);
        }

        public bool CanSetItem(InventoryItemView item) =>  SlotType == ArtifactSlotType.Universal || item == null  || item.ItemConfig.SlotType == SlotType;

        public void SetItem(InventoryItemView item, bool inform = true)
        {
            var old = Item;
            Item = item;
            item?.SetParentSlot(this);
            Highlight(false);
            if(inform) OnNewItemPlaced?.Invoke(item, old);
        }

        public void Highlight(bool active) =>
            _background.color = active ? _selected : _unselected;

        public void OnDrop(PointerEventData eventData)
        {
            InventoryItemView initialItem = eventData.pointerDrag.GetComponent<InventoryItemView>();
            InventorySlotView startSlot = initialItem.ParentSlot;
            InventoryItemView exchangeItem = Item; 

            if(startSlot.CanSetItem(exchangeItem) && CanSetItem(initialItem))
            {
                startSlot.SetItem(exchangeItem);
                SetItem(initialItem);
            }
        }
    }
}
