using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class Artifact : IArtifact, IModifiableArtifact
    {
        public enum ArtifactActivatorTypes
        {
            Modifier = 0,
            Attack = 1,
            Jump = 2,
            Defence = 3,
            ExplicitAction = 10
        }


        protected ParametersModifier<ActorStatTypes> _modifiers;
        protected Dictionary<ArtifactActivatorTypes, ArtifactActor> actors;

        protected IArtifactHolder holder;

        public string NameTag { get; protected set; }
        public string Name => NameTag; //replace with localization
        public Sprite Icon { get; protected set; }
        public ArtifactSlotType SlotType {get; protected set;}


        public List<ICharacterModifier> PassiveCharacterModifiers { get; private set; } = new();


        public Artifact(ItemConfig config)
        {
            if (config == null) return;

            NameTag = config.NameTag;

            SlotType = config.SlotType;
            Icon = config.Icon;

            PassiveCharacterModifiers = config.PassiveCharacterModifiers;

            actors = new();
            foreach(var act in config.Actions)
            {
                ArtifactActor next = new(act);
                if(actors.ContainsKey(next.ActivatorType)) actors[next.ActivatorType].AddInternalActor(next);
                else actors.Add(next.ActivatorType, next);
            }

            _modifiers = new();
        }

        public Artifact()
        {
            NameTag = "";

            actors = new();

            _modifiers = new();
        }

        public virtual void SetHolder(IArtifactHolder holder)
        {
            this.holder = holder;
            foreach(var act in actors.Values)
                act.SetHolder(holder);
        }

        public void TryUseFor(ArtifactActivatorTypes actionType)
        {
            if (holder == null)
            {
                Debug.Log($"Unassigned artifact {NameTag}");
                return;
            }

            if (actors.ContainsKey(actionType))
                actors[actionType].Use();
                //if (ex.IsReady) ex.Use(holder);
        }


        public void SetModifiers(List<IArtifactModifier> modifiers)
        {
            foreach(var m in modifiers)
                _modifiers.AddModifier(m.Type, m.Value);
        }
        public void ClearAllModifiers()
        {
            _modifiers.CancelAllTempEffects();
            _modifiers = new();
        }
    }
}
