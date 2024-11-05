using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class Artifact : IArtifact
    {
        private ItemConfig _config;
        private ArtifactExecutor _executor;

        protected Stat<ArtifactStatTypes> cooldown;

        public string Name => _config.NameTag; //replace with localization
        public Sprite Icon => _config.Icon;
        public Sprite LevelView => _config.LevelView;
        public ArtifactSlotType SlotType => _config.SlotType;
        public bool HasPassiveArtifactModifier => _config.HasPassiveArtifactModifier;
        public bool IsActive => _config.HasActiveExecutor;

        public List<ICharacterModifier> PassiveCharacterModifiers { get; private set; }

        public Artifact(ItemConfig config)
        {
            _config = config;
            _executor = ArtifactExecutorFactory.GetExecutor(_config.NameTag);

            cooldown = new(ArtifactStatTypes.Cooldown, _config.Cooldown);

            PassiveCharacterModifiers = _config.PassiveCharacterModifiers;
        }

        public void Use() => _executor.Use();
        public void UseToModify() { }
    }

    public class Weapon : Artifact, IModifiableArtifact
    {
        protected Stat<ArtifactStatTypes> damage;
        protected Stat<ArtifactStatTypes> actionDistance;
        protected Stat<ArtifactStatTypes> fireForce;
        public Weapon(WeaponConfig config) : base(config)
        {
            damage = new(ArtifactStatTypes.Damage, config.Damage);
            actionDistance = new(ArtifactStatTypes.ActionDistance, config.ActionDistance);
            fireForce = new(ArtifactStatTypes.FireForce, config.FireForce);
            //register ammo in pool
        }
        public void SetModifiers(List<IArtifactModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
                GetStatByType(modifiers[i].Type).AddModifier(modifiers[i].Value);
        }
        public void ClearAllModifiers()
        {
            cooldown.ClearModifier();
            damage.ClearModifier();
            actionDistance.ClearModifier();
            fireForce.ClearModifier();
        }

        private Stat<ArtifactStatTypes> GetStatByType(ArtifactStatTypes type) => type switch
        {
            ArtifactStatTypes.Cooldown => cooldown,
            ArtifactStatTypes.Damage => damage,
            ArtifactStatTypes.ActionDistance => actionDistance,
            ArtifactStatTypes.FireForce => fireForce,
            _ => new(ArtifactStatTypes.Cooldown)
        };
    }
}
