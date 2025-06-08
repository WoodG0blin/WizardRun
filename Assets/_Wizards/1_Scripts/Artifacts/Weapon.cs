using UnityEngine;

namespace WizardsPlatformer
{
    public interface IWeapon : IArtifact
    {
        public bool IsReady { get; }
        public int Damage { get; }
        public float FireForce { get; }
        public float Distance { get; }
        void Fire(Vector2 direction, float force = 0);
    }

    public class Weapon : Artifact, IWeapon
    {
        protected ArtifactActor attackActor;

        public bool IsReady => attackActor.IsReady;
        public int Damage => attackActor.ActionValue;
        public float FireForce => attackActor.ActionSpeed;
        public float Distance => attackActor.ActionDistance;


        public Weapon(ItemConfig config) : base(config)
        {
            if (actors[ArtifactActivatorTypes.Attack] == null)
            {
                Debug.Log($"Incorrect weapon config for {NameTag}");
                return;
            }

            attackActor = actors[ArtifactActivatorTypes.Attack];
            actors[ArtifactActivatorTypes.Attack] = null;
        }


        public Weapon(ActorStatsConfig actionConfig)
        {
            attackActor = new(actionConfig);
            actors = new();
        }

        public override void SetHolder(IArtifactHolder holder)
        {
            base.SetHolder(holder);
            attackActor.SetHolder(holder);
        }

        public void Fire(Vector2 direction, float force = 0)
        {
            if (attackActor.IsReady)
            {
                attackActor.Use();
                attackActor.TriggerCooldown();
            }
        }
    }
}
