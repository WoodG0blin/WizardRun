using System.Collections.Generic;

namespace WizardsPlatformer
{
    public class ActiveArtifact : Artifact, IModifiableArtifact
    {
        protected Stat<ArtifactStatTypes> damage;
        protected Stat<ArtifactStatTypes> actionDistance;
        protected Stat<ArtifactStatTypes> fireForce;

        public ActiveArtifact(WeaponConfig config) : base(config)
        {
            damage = new(ArtifactStatTypes.Damage, config.Damage);
            actionDistance = new(ArtifactStatTypes.ActionDistance, config.ActionDistance);
            fireForce = new(ArtifactStatTypes.FireForce, config.FireForce);
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
