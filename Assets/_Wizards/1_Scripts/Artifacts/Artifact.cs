using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifact : IItem
    {
        void Equip(IArtifactHolder holder);
        void Unequip();
    }

    public class Artifact : IArtifact
    {
        public enum ArtifactActivatorTypes
        {
            Modifier = 0,
            Attack = 1,
            Jump = 2,
            Defence = 3,
            ExplicitAction = 10
        }


        //protected ParametersModifier<ActorStatTypes> _modifiers;
        //public List<ArtifactActor> Actors { get; protected set; }
        protected List<ArtifactProperty> properties = new();

        //protected IArtifactHolder holder;

        public string NameTag { get; protected set; }
        public string Name => NameTag; //replace with localization
        public Sprite Icon { get; protected set; }
        public ArtifactSlotType SlotType {get; protected set;}


        //public List<ICharacterModifier> PassiveCharacterModifiers { get; private set; } = new();


        public Artifact(ItemConfig config)
        {
            if (config == null) return;

            NameTag = config.NameTag;

            SlotType = config.SlotType;
            Icon = config.Icon;

            properties.Add(new(config.BaseProperty, NameTag, isBaseProperty: true));
            foreach (var conf in config.ExtraProperties)
                properties.Add(new(conf, NameTag));


            //LEGACY

            //PassiveCharacterModifiers = config.PassiveCharacterModifiers;

            //Actors = new();
            //bool needAttack = SlotType == ArtifactSlotType.Weapon;
            //foreach(var act in config.Actions)
            //{
            //    switch(act.ActivatorType)
            //    {
            //        case ArtifactActivatorTypes.Attack:
            //            {
            //                Actors.Add(needAttack ? new AttackActor(act) : new ArtifactActor(act));
            //                needAttack = false;
            //                break;
            //            }
            //        case ArtifactActivatorTypes.Modifier:
            //            {
            //                Actors.Add(new ModifierActor(act));
            //                break;
            //            }
            //        default:
            //            {
            //                Actors.Add(new ArtifactActor(act));
            //                break;
            //            }
            //    }
            //}

            //_modifiers = new();
        }

        public void Equip(IArtifactHolder holder)
        {
            foreach(var prop in properties)
                prop.Init(holder);
        }

        public void Unequip()
        {
            foreach (var prop in properties)
                prop.DeInit();
        }

        //public virtual void SetHolder(IArtifactHolder holder)
        //{
        //    this.holder = holder;
        //    foreach(var act in Actors)
        //        act.SetHolder(holder);
        //}

        //public void TryUseFor(ArtifactActivatorTypes actionType)
        //{
        //    //if (holder == null)
        //    //{
        //    //    Debug.Log($"Unassigned artifact {NameTag}");
        //    //    return;
        //    //}

        //    //if (actors.ContainsKey(actionType))
        //    //    actors[actionType].Use();
        //    //    //if (ex.IsReady) ex.Use(holder);
        //}


        //public void SetModifiers(List<IArtifactModifier> modifiers)
        //{
        //    foreach(var m in modifiers)
        //        _modifiers.AddModifier(m.Type, m.Value);
        //}
        //public void ClearAllModifiers()
        //{
        //    _modifiers.CancelAllTempEffects();
        //    _modifiers = new();
        //}

        //public virtual ArtifactActor GetActor(ArtifactActivatorTypes type)
        //{
        //    //if (actors.ContainsKey(type)) return actors[type];
        //    //else return null;
        //    return null;
        //}
    }
}
