using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IItem
    {
        string ID { get; }
        string Name { get; }
        Sprite Icon { get; }
        Sprite LevelView { get; }
        bool IsUpgrade { get; }
        UpgradeConfig Upgrade { get; }
    }

    [CreateAssetMenu(fileName = "New" + nameof(ItemConfig), menuName = "Configs/" + nameof(ItemConfig), order = 2)]
    internal sealed class ItemConfig : ScriptableObject, IItem
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Sprite LevelView { get; private set; }
        [field: SerializeField] public bool IsUpgrade { get; private set; }

        [field: SerializeField] public UpgradeConfig Upgrade { get; private set; }
    }
}