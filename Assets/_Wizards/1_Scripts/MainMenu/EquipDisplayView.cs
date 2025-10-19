using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class EquipDisplayView : MenuPanelView
    {
        [SerializeField] private List<InventorySlotView> _equipSlots;

        protected override void OnInit()
        {
            foreach (var slot in _equipSlots)
            {
                slot.Init();
                slot.OnNewItemPlaced = i => EquipArtifact(slot.SlotType, i);
            }
        }

        private void EquipArtifact(ArtifactSlotType slotType, InventoryItemView item)
        {
            menuInfo.PlayerModel.EquipArtifact(slotType, item != null ? item.ItemConfig : null);
        }

        public void Display(List<InventoryItemView> equippedArtifacts)
        {
            foreach (var a in equippedArtifacts)
            {
                for(int i = 0; i < _equipSlots.Count; i++)
                {
                    if (_equipSlots[i].CanSetItem(a) && _equipSlots[i].Item != null)
                        _equipSlots[i].SetItem(a);
                }
            }
        }

        public void HighlightSlot(ArtifactSlotType slotType)
        {
            foreach (var slot in _equipSlots)
                slot.Highlight(slot.SlotType == slotType);
        }
    }
}
