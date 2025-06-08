using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

namespace WizardsPlatformer
{
    public class ArtifactActor
    {
        protected IArtifactHolder holder;

        public Artifact.ArtifactActivatorTypes ActivatorType { get; protected set; }
        public string NameTag { get; protected set; }

        protected int actionValue;
        protected float actionDistance;
        protected float actionSpeed;
        
        protected int cooldown;

        private ArtifactExecutor _executor;
        private ParametersModifier<ActorStatTypes> _modifiers = new();

        public int ActionValue => actionValue + _modifiers.GetModifier(ActorStatTypes.Value);
        public float ActionDistance => actionDistance + _modifiers.GetModifier(ActorStatTypes.Distance);
        public float ActionSpeed => actionSpeed + _modifiers.GetModifier(ActorStatTypes.Speed);
        public int Cooldown => cooldown + _modifiers.GetModifier(ActorStatTypes.Cooldown);

        public bool IsReady => RemainingCooldown <= 0;
        public float RemainingCooldown { get; protected set; }
        protected Coroutine cooldownTimer;


        public GameObject Ammo { get; protected set; }
        
        public bool IsBallistic { get; protected set; }
        public bool IsRanged => actionDistance > 1;

        public ArtifactActor InternalActor { get; protected set; }


        public ArtifactActor(ActorStatsConfig config)
        {
            ActivatorType = config.ActivatorType;
            NameTag = config.NameTag;

            actionValue = config.ActionValue;
            actionDistance = config.ActionDistance;
            actionSpeed = config.ActionSpeed;

            cooldown = config.CoolDown;

            Ammo = config.Ammo;

            _executor = ArtifactExecutorFactory.GetExecutor(ActivatorType, NameTag);
            _executor.Init(this);
        }

        public void SetHolder(IArtifactHolder holder) => this.holder = holder;

        public void Use()
        {
            _executor.Use(holder);
        }

        public void TriggerCooldown() =>
            cooldownTimer = holder.SetTimer(Cooldown, t => RemainingCooldown = t, cooldownTimer);

        public void SetModifiers(List<IArtifactModifier> modifiers, int seconds = 0)
        {
            foreach (var m in modifiers)
                if(seconds > 0) _modifiers.AddModifierTemp(m.Type, m.Value, seconds);
                else _modifiers.AddModifier(m.Type, m.Value);
        }
        public void ClearAllModifiers()
        {
            _modifiers.CancelAllTempEffects();
            _modifiers = new();
        }

        public void AddInternalActor(ArtifactActor nextActor)
        {
            if (InternalActor == null) InternalActor = nextActor;
            else InternalActor.AddInternalActor(nextActor);
        }

        public void RemoveInternalActor(ArtifactActor actor)
        {
            if (InternalActor == null) return;

            if (InternalActor.NameTag == actor.NameTag) InternalActor = InternalActor.InternalActor;
            else InternalActor.RemoveInternalActor(actor);
        }
    }


}