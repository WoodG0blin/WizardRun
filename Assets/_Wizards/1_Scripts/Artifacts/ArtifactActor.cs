using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    public class ArtifactActor
    {
        protected IArtifactHolder holder;

        public Artifact.ArtifactActivatorTypes ActivatorType { get; protected set; }
        public string NameTag { get; protected set; }

        protected int actionValue;
        protected int actionDistance;
        protected int actionSpeed;
        
        protected int cooldown;

        protected ArtifactExecutor executor;
        private ParametersModifier<ActorStatTypes> _modifiers = new();

        public int ActionValue => actionValue + _modifiers.GetModifier(ActorStatTypes.Value);
        public int ActionDistance => actionDistance + _modifiers.GetModifier(ActorStatTypes.Distance);
        public int ActionSpeed => actionSpeed + _modifiers.GetModifier(ActorStatTypes.Speed);
        public int Cooldown => cooldown + _modifiers.GetModifier(ActorStatTypes.Cooldown);

        public bool IsMain { get; protected set; }

        public bool IsReady => RemainingCooldown <= 0;
        public float RemainingCooldown { get; protected set; }
        protected Coroutine cooldownTimer;


        public GameObject Ammo { get; protected set; }
        
        public bool IsBallistic { get; protected set; }
        public bool IsRanged => actionDistance > 1;

        public ArtifactActor InternalActor { get; protected set; }


        public ArtifactActor(ActorStatsConfig config, bool isMain = false)
        {
            ActivatorType = config.ActivatorType;
            NameTag = config.NameTag;

            actionValue = config.ActionValue;
            actionDistance = config.ActionDistance;
            actionSpeed = config.ActionSpeed;

            cooldown = config.CoolDown;

            Ammo = config.Ammo;

            executor = ArtifactExecutorFactory.GetExecutor(ActivatorType, NameTag);
            executor.Init(this);
            IsMain = isMain;
        }

        public void SetHolder(IArtifactHolder holder) => this.holder = holder;

        public virtual void Set(List<ArtifactActor> holderActors)
        {
            var match = holderActors.Where(a => a.ActivatorType == ActivatorType).FirstOrDefault();
            if (match != null && ActivatorType != Artifact.ArtifactActivatorTypes.ExplicitAction) match.AddInternalActor(this);
            else holderActors.Add(this);
        }


        public virtual void Use()
        {
            executor?.Use(holder);
            InternalActor?.Use();
        }

        public void TriggerCooldown() =>
            cooldownTimer = holder.SetTimer(Cooldown, t => RemainingCooldown = t, cooldownTimer);

        public void SetModifier(ActorStatTypes type, int value, int seconds = 0)
        {
            if(seconds > 0) _modifiers.AddModifierTemp(type, value, seconds);
            else _modifiers.AddModifier(type, value);
        }
        public void ClearAllModifiers()
        {
            _modifiers.CancelAllTempEffects();
            _modifiers = new();
            InternalActor = null;
        }

        public void AddInternalActor(ArtifactActor nextActor)
        {
            if (InternalActor == null)
            {
                InternalActor = nextActor;
                InternalActor.SetHolder(holder);
            }
            else InternalActor.AddInternalActor(nextActor);
        }

        public void RemoveInternalActor(ArtifactActor actor)
        {
            if (InternalActor == null) return;

            if (InternalActor.NameTag == actor.NameTag) InternalActor = InternalActor.InternalActor;
            else InternalActor.RemoveInternalActor(actor);
        }
    }

    public class AttackActor : ArtifactActor, IWeapon
    {
        public AttackActor(ActorStatsConfig config) : base(config, isMain: true)
        {
            executor = null;
        }

        public int Damage => ActionValue;
        public float FireForce => ActionSpeed;
        public float Distance => ActionDistance;


        public override void Set(List<ArtifactActor> holderActors)
        {
            var match = holderActors.Where(a => a.ActivatorType == ActivatorType).FirstOrDefault();
            if (match != null)
            {
                InternalActor = match;
                match = this;
            }
            else holderActors.Add(this);
        }

        public override void Use()
        {

            if (actionDistance > 1) RangedAttack(holder);
            else MeleeAttack(holder);

            TriggerCooldown();

            base.Use();
        }

        private void MeleeAttack(IArtifactHolder holder)
        {
            IInteractionResponder hit = holder.GetTargetAt(ActionDistance);

            if (hit == null) return;

            if (holder.IsPlayer ^ hit.IsPlayer) hit.ReceiveDamage(ActionValue);
        }

        private void RangedAttack(IArtifactHolder holder)
        {
            //Debug.Log($"Using ranged attack. Ammo set? {Ammo != null}");
            Ammo bullet = new Ammo(ActionValue, prefab: Ammo);
            holder.PlaceAmmo(bullet);
            bullet.SetToPlayer(holder.IsPlayer);
            bullet.Fire(holder.Direction, ActionSpeed);
        }
    }

    public class ModifierActor : ArtifactActor
    {
        public ModifierActor(ActorStatsConfig config) : base(config)
        {
            executor = null;
        }

        public override void Set(List<ArtifactActor> holderActors)
        {
            foreach (var act in holderActors) CheckActorsChain(act);
        }

        private void CheckActorsChain(ArtifactActor actor)
        {
            if(actor.NameTag == NameTag)
            {
                actor.SetModifier(ActorStatTypes.Cooldown, Cooldown);
                actor.SetModifier(ActorStatTypes.Value, ActionValue);
                actor.SetModifier(ActorStatTypes.Distance, ActionDistance);
                actor.SetModifier(ActorStatTypes.Speed, ActionSpeed);
            }

            if (actor.InternalActor == null) return;
            else CheckActorsChain(actor.InternalActor);
        }
    }
}