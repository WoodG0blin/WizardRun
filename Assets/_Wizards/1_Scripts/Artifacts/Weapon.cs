using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IWeapon
    {
        public void SetHolder(IArtifactHolder holder);
        public bool IsReady { get; }
        public int Damage { get; }
        public float FireForce { get; }
        public float Distance { get; }
        void Use();
    }

    //public class Weapon : Artifact, IWeapon
    //{
    //    protected AttackActor attackActor;

    //    public bool IsReady => attackActor.IsReady;
    //    public int Damage => attackActor.ActionValue;
    //    public float FireForce => attackActor.ActionSpeed;
    //    public float Distance => attackActor.ActionDistance;


    //    public Weapon(ItemConfig config) : base(config)
    //    {
    //        var attackConfig = config.Actions.Where(c => c.ActivatorType == ArtifactActivatorTypes.Attack).FirstOrDefault();
    //        if (attackConfig == null)
    //        {
    //            Debug.Log($"Incorrect weapon config for {NameTag}");
    //            return;
    //        }

    //        attackActor = new(attackConfig);
    //        //actors[ArtifactActivatorTypes.Attack] = null;
    //    }


    //    public Weapon(ActorStatsConfig actionConfig)
    //    {
    //        attackActor = new(actionConfig);
    //        Actors = new();
    //    }

    //    public override void SetHolder(IArtifactHolder holder)
    //    {
    //        base.SetHolder(holder);
    //        attackActor.SetHolder(holder);
    //    }

    //    public void Fire(Vector2 direction, float force = 0)
    //    {
    //        if (attackActor.IsReady)
    //        {
    //            attackActor.Use();
    //            attackActor.TriggerCooldown();
    //        }
    //    }

    //    public override ArtifactActor GetActor(ArtifactActivatorTypes type)
    //    {
    //        if (type == ArtifactActivatorTypes.Attack) return attackActor;
    //        else return base.GetActor(type);
    //    }
    //}
}
