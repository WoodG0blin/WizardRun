using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IItem
    {
        string NameTag { get; }
        string Name { get; }
        Sprite Icon { get; }
        Sprite LevelView { get; }
        bool IsUpgrade { get; }
        public ArtifactSlotType SlotType { get; }
        UpgradeConfig Upgrade { get;}
    }

    [CreateAssetMenu(fileName = "New" + nameof(ItemConfig), menuName = "Configs/" + nameof(ItemConfig), order = 2)]
    internal sealed class ItemConfig : ScriptableObject, IItem
    {
        public interface IModifier
        {
            public CharacterStatType Type { get; }
            public int Value { get; }
        }
        [Serializable]
        private struct Modifier : IModifier
        {
            [SerializeField] public CharacterStatType Type { get; private set; }
            [SerializeField] public int Value { get; private set;}
        }


        [field: Header("GENERAL"), Space(10)]
        [field: SerializeField] public string NameTag { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Sprite LevelView { get; private set; }
        [field: SerializeField] public ArtifactSlotType SlotType { get; private set; }

        [field: Header("PASSIVE"), Space(10)]
        [SerializeField] private List<Modifier> _passiveCharacterModifiers;
        [field: SerializeField] public bool HasPassiveArtifactmodifier { get; private set; }

        [field: Header("ACTIVE"), Space(10)]
        [field: SerializeField] public UpgradeConfig Upgrade { get; private set; }


        public string Name => NameTag;
        public bool IsUpgrade => Upgrade != null;
        public List<IModifier> PassiveCharacterModifiers => _passiveCharacterModifiers.Cast<IModifier>().ToList();
    }
}