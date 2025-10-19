using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IItem : IDisplayInfo
    {
        string NameTag { get; }
        public ArtifactSlotType SlotType { get; }
    }

    public interface IModifier<T> where T :struct, IConvertible
    {
        public T Type { get; }
        public int Value { get; }
    }

    public interface ICharacterModifier : IModifier<CharacterStatType> { }
    public interface IArtifactModifier : IModifier<ActorStatTypes> { }



    [CreateAssetMenu(fileName = "New" + nameof(ItemConfig), menuName = "Configs/" + nameof(ItemConfig), order = 2)]
    public class ItemConfig : ScriptableObject, IItem
    {
        [Serializable]
        protected struct Modifier : ICharacterModifier
        {
            [field: SerializeField] public CharacterStatType Type { get; private set; }
            [field: SerializeField] public int Value { get; private set;}
        }


        [field: Header("GENERAL")]
        [field: SerializeField] public string NameTag { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; protected set; }
        [field: SerializeField] public ArtifactSlotType SlotType { get; protected set; }
        [field: SerializeField] public ArtifactPropertyConfig BaseProperty { get; set; }
        [field: SerializeField] public List<ArtifactPropertyConfig> ExtraProperties { get; set; }
        public int ControlIndex { get; set; } = -1;

        public string Name => NameTag;
        public bool HasExplicitProperty =>
            BaseProperty.ActivatorType == PropertyActivators.Explicit
            || ExtraProperties.Where(p => p.ActivatorType == PropertyActivators.Explicit).FirstOrDefault() != null;
    }
}