using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class EquipDisplayView : MonoBehaviour
    {
        [SerializeField] private InventorySlotView _weaponSlot;
        [SerializeField] private InventorySlotView _headSlot;
        [SerializeField] private InventorySlotView _neckSlot;
        [SerializeField] private InventorySlotView _waistSlot;
        [SerializeField] private InventorySlotView _legsSlot;

        private Dictionary<ArtifactSlotType, InventorySlotView> _slots;

        public void Init(Transform mainContainer, Func<InventoryItemView, ArtifactSlotType, bool> tryEquipItemToSlot)
        {
            _slots = new()
            {
                { ArtifactSlotType.Weapon, _weaponSlot },
                { ArtifactSlotType.Head, _headSlot },
                { ArtifactSlotType.Neck, _neckSlot },
                { ArtifactSlotType.Waist, _waistSlot },
                { ArtifactSlotType.Legs, _legsSlot}
            };

            foreach (var kvp in _slots)
                kvp.Value.Init(
                    mainContainer,
                    (i) => tryEquipItemToSlot(i, kvp.Key));
        }

        public void Display(List<InventoryItemView> equippedArtifacts)
        {
            foreach (var a in equippedArtifacts)
                _slots[a.SlotType].TrySetItem(a);
        }

        public void HighlightSlot(ArtifactSlotType slot)
        {
            foreach (var kvp in _slots)
                kvp.Value.Highlight(kvp.Key == slot);
        }
    }
}
