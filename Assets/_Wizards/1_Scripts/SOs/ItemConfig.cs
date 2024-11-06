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
        //bool IsUpgrade { get; }
        public ArtifactSlotType SlotType { get; }
        //UpgradeConfig Upgrade { get;}
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
        private struct Modifier : ICharacterModifier
        {
            [field: SerializeField] public CharacterStatType Type { get; private set; }
            [field: SerializeField] public int Value { get; private set;}
        }


        [field: Header("GENERAL")]
        [field: SerializeField] public string NameTag { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Sprite LevelView { get; private set; }
        [field: SerializeField] public ArtifactSlotType SlotType { get; private set; }

        [field: Header("PASSIVE")]
        [SerializeField] private List<Modifier> _passiveCharacterModifiers;
        
        [field: Header("EXECUTORS")]
        [field: SerializeField] public List<ArtifactExecutorType> ArtifactExecutors { get; private set; }
        [field: SerializeField] public int Cooldown { get; private set; }
        
        [field: Header("ACTION CONFIGS - OPTIONAL")]
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int ActionDistance { get; private set; }
        [field: SerializeField] public int FireForce { get; private set; }
        [field: SerializeField] public GameObject Ammo { get; private set; }


        //[field: SerializeField] public UpgradeConfig Upgrade { get; private set; }


        public string Name => NameTag;
        //public bool IsUpgrade => Upgrade != null;
        public List<ICharacterModifier> PassiveCharacterModifiers => _passiveCharacterModifiers.Cast<ICharacterModifier>().ToList();
    }
}