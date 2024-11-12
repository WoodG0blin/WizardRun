using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class InventoryItemView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Image _icon;
        public InventorySlotView ParentSlot { get; private set; }
        public ArtifactSlotType SlotType {get; private set;}

        private Action _displayDescription;
        
        public void Init(Sprite icon, ArtifactSlotType slotType, Action displayDescription)
        {
            SlotType = slotType;
            _icon.sprite = icon;
            _displayDescription = displayDescription;
            gameObject.SetActive(true);
        }

        public void SetParentSlot(InventorySlotView parentSlot)
        {
            ParentSlot = parentSlot;
            transform.SetParent(parentSlot.Container);
            transform.localScale = Vector3.one;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(ParentSlot.MainContainer);
            transform.SetAsLastSibling();
            _icon.raycastTarget = false;
            _displayDescription();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ParentSlot.TrySetItem(this);
            _icon.raycastTarget = true;
            //_displayDescription(false);
        }
    }
}
