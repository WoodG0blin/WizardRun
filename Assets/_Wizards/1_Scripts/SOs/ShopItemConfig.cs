using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = "New" + nameof(ShopItemConfig), menuName = "Configs/" + nameof(ShopItemConfig))]
    public class ShopItemConfig : ScriptableObject
    {
        [field: SerializeField] public int Price { get; set; }
        [field: SerializeField] public Sprite DisplayImage { get; set; }
        [field: SerializeField] public ItemSO Item { get; set; }
        [field: SerializeField] public Bonus Bonus { get; set; }
    }
}