using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IInventoryModel
    {
        IReadOnlyList<string> EquippedItems { get; }
        void EquipItem(string ItemID);
        void UnEquipItem(string ItemID);
        bool IsEquipped(string ItemID);
    }

    public class InventoryModel : IInventoryModel
    {
        private readonly List<string> _equippedItems = new List<string>();
        public IReadOnlyList<string> EquippedItems => _equippedItems;

        public void EquipItem(string ItemID)
        {
            if (!IsEquipped(ItemID)) _equippedItems.Add(ItemID);
        }

        public bool IsEquipped(string ItemID) => _equippedItems.Contains(ItemID);

        public void UnEquipItem(string ItemID)
        {
            if (IsEquipped(ItemID)) _equippedItems.Remove(ItemID);
        }
    }
}