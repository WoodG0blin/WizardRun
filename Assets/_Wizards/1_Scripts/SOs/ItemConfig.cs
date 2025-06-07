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
        public ArtifactSlotType SlotType { get; }
    }

    public interface IModifier<T> where T :struct, IConvertible
    {
        public T Type { get; }
        public int Value { get; }
    }

    public interface ICharacterModifier : IModifier<CharacterStatType> { }
    public interface IArtifactModifier : IModifier<ArtifactStatTypes> { }



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

        [field: Header("PASSIVE")]
        [SerializeField] protected List<Modifier> _passiveCharacterModifiers;
        
        [field: Header("EXECUTORS")]
        [field: SerializeField] public List<Artifact.ExecutorType> Actions { get; protected set; }
        [field: SerializeField] public int Cooldown { get; protected set; }
        
        [field: Header("ACTION CONFIGS - OPTIONAL")]
        [field: SerializeField] public int ActionValue { get; protected set; }
        [field: SerializeField] public float ActionDistance { get; protected set; }

        public string Name => NameTag;
        public List<ICharacterModifier> PassiveCharacterModifiers => _passiveCharacterModifiers.Cast<ICharacterModifier>().ToList();
    }
}