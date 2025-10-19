using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifact : IItem
    {
        void Equip(IArtifactHolder holder);
        void Unequip();
        ItemConfig Config { get; }
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

        protected List<ArtifactProperty> properties = new();

        public string NameTag { get; protected set; }
        public string Name => NameTag; //replace with localization
        public Sprite Icon { get; protected set; }
        public ArtifactSlotType SlotType {get; protected set;}
        public ItemConfig Config { get; protected set; }


        public Artifact(ItemConfig config)
        {
            if (config == null) return;

            Config = config;
            NameTag = config.NameTag;

            SlotType = config.SlotType;
            Icon = config.Icon;

            properties.Add(new(config.BaseProperty, NameTag, isBaseProperty: true));
            foreach (var conf in config.ExtraProperties)
                properties.Add(new(conf, NameTag));
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
    }
}
