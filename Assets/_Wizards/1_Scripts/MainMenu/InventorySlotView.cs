using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class InventorySlotView : MonoBehaviour, IDropHandler
    {
        public InventoryItemView Item { get; private set; }
        [field: SerializeField] public Transform Container { get; private set; }

        [SerializeField] private Image _background;

        [SerializeField] private Color _selected;
        [SerializeField] private Color _unselected;

        private Func<InventoryItemView, bool> _checkUpdateCondition;

        public Transform MainContainer { get; private set; }

        public void Init(Transform mainContainer, Func<InventoryItemView, bool> checkSlotUpdateCondition = null)
        {
            MainContainer = mainContainer;

            gameObject.SetActive(true);
            _checkUpdateCondition = checkSlotUpdateCondition;

            Highlight(false);

            if(Item!= null) GameObject.Destroy(Item.gameObject);
        }

        public bool TrySetItem(InventoryItemView item)
        {
            bool check =
                _checkUpdateCondition != null ?
                    _checkUpdateCondition(item) : true;

            if(check)
            {
                Item = item;
                item?.SetParentSlot(this);
                Highlight(false);
            }

            return check;
        }

        public void Highlight(bool active) =>
            _background.color = active ? _selected : _unselected;

        public void OnDrop(PointerEventData eventData)
        {
            var drop = eventData.pointerDrag.GetComponent<InventoryItemView>();

            var prevItem = Item;
            var dropParent = drop.ParentSlot;

            if (!TrySetItem(drop) || !dropParent.TrySetItem(prevItem))
            {
                TrySetItem(prevItem);
                drop.SetParentSlot(dropParent);
            }
        }
    }
}
