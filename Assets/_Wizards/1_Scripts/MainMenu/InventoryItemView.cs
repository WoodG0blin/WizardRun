using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class InventoryItemView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        public InventorySlotView ParentSlot { get; private set; }
        public ItemConfig ItemConfig { get; private set; }

        private Action<ItemConfig> _displayDescription;

        private Transform _container;
        
        public void Init(ItemConfig item, Action<ItemConfig> displayDescription, Transform container)
        {
            ItemConfig = item;
            _icon.sprite = ItemConfig.Icon;
            _displayDescription = displayDescription;
            gameObject.SetActive(true);

            _container = container;
        }

        public void SetParentSlot(InventorySlotView parentSlot)
        {
            ParentSlot = parentSlot;
            transform.SetParent(parentSlot.ItemPlace);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            var rt = transform.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.anchoredPosition = Vector2.zero;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_container);
            transform.SetAsLastSibling();
            _icon.raycastTarget = false;
            _displayDescription(ItemConfig);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //ParentSlot.SetItem(this);
            SetParentSlot(ParentSlot);
            _icon.raycastTarget = true;
            _displayDescription(null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _displayDescription(ItemConfig);
        }
    }
}
