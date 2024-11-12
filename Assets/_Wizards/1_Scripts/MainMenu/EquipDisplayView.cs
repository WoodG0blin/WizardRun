using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class EquipDisplayView : MonoBehaviour
    {
        [SerializeField] private InventoryItemView _weaponSlot;
        [SerializeField] private InventoryItemView _headSlot;
        [SerializeField] private InventoryItemView _neckSlot;
        [SerializeField] private InventoryItemView _waistSlot;
        [SerializeField] private InventoryItemView _legsSlot;

        private Dictionary<ArtifactSlotType, InventoryItemView> _slots;

        public void Init()
        {
            _slots = new()
            {
                { ArtifactSlotType.Weapon, _weaponSlot },
                { ArtifactSlotType.Head, _headSlot },
                { ArtifactSlotType.Neck, _neckSlot },
                { ArtifactSlotType.Waist, _waistSlot },
                { ArtifactSlotType.Legs, _legsSlot}
            };
        }

        public void Display(List<IArtifact> equippedArtifacts, Action<IArtifact> onRemoveItem)
        {
            Clear();

            foreach (var a in equippedArtifacts)
                SetItem(a, onRemoveItem);
        }

        public void SetItem(IArtifact item, Action<IArtifact> onRemoveItem) =>
            _slots[item.SlotType].Init(item, () => onRemoveItem?.Invoke(item));

        private void Clear()
        {
            foreach (var slot in _slots.Values) slot.Clear();
        }
    }
}
