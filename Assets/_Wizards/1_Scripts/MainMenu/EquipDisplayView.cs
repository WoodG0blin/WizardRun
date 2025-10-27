using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class EquipDisplayView : MenuPanelView
    {
        [SerializeField] private List<InventorySlotView> _equipSlots;
        [SerializeField] private ControlAllocationView _controlsAllocation;

        protected override void OnInit()
        {
            foreach (var slot in _equipSlots)
            {
                slot.Init();
                slot.OnNewItemPlaced = EquipArtifact;
            }
            _controlsAllocation.Init(menuInfo);
        }

        protected override void OnActivation()
        {
            _controlsAllocation.SetActive(false);
        }

        private void EquipArtifact(InventoryItemView placed, InventoryItemView removed)
        {
            ItemConfig newItem = placed != null ? placed.ItemConfig : null;
            ItemConfig removedItem = removed != null ? removed.ItemConfig : null;

            menuInfo.PlayerModel.EquipArtifact(newItem, removedItem);

            if (newItem != null && newItem.HasExplicitProperty)
            {
                if (newItem.SlotType == ArtifactSlotType.Weapon) newItem.ControlIndex = 0;
                else _controlsAllocation.Display(UpdateControlAllocations, newItem);
            }

            //HighlightSlot(ArtifactSlotType.Universal);
        }

        private void UpdateControlAllocations(List<ItemConfig> changedItems)
        {
            _controlsAllocation.SetActive(false);

            foreach(var item in changedItems)
                menuInfo.PlayerModel.EquipArtifact(item, item);
        }


        public void Display(List<InventoryItemView> equippedArtifacts)
        {
            foreach (var a in equippedArtifacts)
            {
                for(int i = 0; i < _equipSlots.Count; i++)
                {
                    if (_equipSlots[i].CanSetItem(a) && _equipSlots[i].Item == null)
                    {
                        _equipSlots[i].SetItem(a, false);
                        break;
                    }
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
