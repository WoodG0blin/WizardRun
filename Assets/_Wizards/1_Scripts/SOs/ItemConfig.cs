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

        //LEGACY

        //[field: Header("PASSIVE")]
        //[SerializeField] protected List<Modifier> _passiveCharacterModifiers;
        
        //[field: Header("ACTIVE")]
        //[field: SerializeField] public List<ActorStatsConfig> Actions { get; protected set; }
        
        //public List<ICharacterModifier> PassiveCharacterModifiers => _passiveCharacterModifiers.Cast<ICharacterModifier>().ToList();
        public string Name => NameTag;
    }


    //[Serializable]
    //public class ActorStatsConfig
    //{
    //    [field: SerializeField] public Artifact.ArtifactActivatorTypes ActivatorType { get; protected set; }

    //    [field: Space(10)]
    //    [field: SerializeField] public int ActionValue { get; protected set; }
    //    [field: SerializeField] public int ActionDistance { get; protected set; }
    //    [field: SerializeField] public int ActionSpeed { get; protected set; }

    //    [field: Space(10)]
    //    [field: SerializeField] public int CoolDown { get; protected set; }

    //    [field: Space(10)]
    //    [field: SerializeField] public GameObject Ammo { get; protected set; }

    //    [field: Space(10)]
    //    [field: SerializeField] public bool IsBallistic { get; protected set; }
    //    [field: SerializeField] public string NameTag { get; protected set; }
    //}
}