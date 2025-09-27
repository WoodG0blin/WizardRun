using System;
using UnityEngine;

namespace WizardsPlatformer
{
    public enum PropertyActivators
    {
        Passive = 0,

        Explicit = 10,

        OnAttack = 20,
        OnJump = 21,
        OnDefence = 22
    }

    public enum PropertyExecution
    {
        Direct = 0,
        BaseParameters = 10,
        Modifier = 20
    }

    [Serializable]
    public class ArtifactPropertyConfig
    {
        [field: SerializeField] public PropertyActivators ActivatorType { get; set; }
        [field: SerializeField] public PropertyExecution ExecutionType { get; set; }
        [field: SerializeField] public CharacterStatType TargetParameter { get; set; }

        [field: Space(10)]
        [field: SerializeField] public int ActionValue { get; set; }
        [field: SerializeField] public int CoolDown { get; set; }

        [field: Space(10)]
        [field: SerializeField] public GameObject View { get; set; }
        [field: SerializeField] public AmmoConfig Ammo { get; set; }
        [field: SerializeField] public string NameTag { get; set; }
    }

    public class ArtifactProperty : IArtifactExecutor
    {
        private bool _isBaseProperty;
        private ParametersModifier<ActorStatTypes> _internalModifiers = new();

        protected IArtifactHolder holder;

        protected ArtifactPropertyExecutor executor;

        protected int baseActionValue;
        public int ActionValue => baseActionValue + _internalModifiers.GetModifier(ActorStatTypes.Value);
        
        protected int baseCooldown;
        protected Coroutine cooldownTimer;
        public int Cooldown => baseCooldown + _internalModifiers.GetModifier(ActorStatTypes.Cooldown);
        public float RemainingCooldown { get; protected set; }
        public bool IsReady => RemainingCooldown <= 0;


        public PropertyActivators ActivatorType { get; protected set; }
        public PropertyExecution ExecutionType { get; protected set; }
        public CharacterStatType TargetParameter { get; protected set; }
        
        public ArtifactPropertyConfig Config { get; protected set; }
        public string NameTag { get; protected set; }

        public AmmoConfig Ammo => Config.Ammo;


        public ArtifactProperty(ArtifactPropertyConfig config, string artName, bool isBaseProperty = false)
        {
            NameTag = artName;
            Config = config;

            ActivatorType = config.ActivatorType;
            ExecutionType = config.ExecutionType;
            TargetParameter = config.TargetParameter;

            baseActionValue = config.ActionValue;

            baseCooldown = config.CoolDown;

            _isBaseProperty = isBaseProperty;

            executor = ActivatorType switch
            {
                PropertyActivators.Explicit => new AttackExecutor(this),
                _ => new(this)
            };
        }

        public void Init(IArtifactHolder holder)
        {
            this.holder = holder;

            executor.Init(holder);

            if (ActivatorType == PropertyActivators.Passive)
                holder.Stats.AddStatsModifier(TargetParameter, ActionValue);
            else holder.Actions.AddAction(ActivatorType, this, _isBaseProperty);
        }

        public void DeInit()
        {
            if (ActivatorType == PropertyActivators.Passive)
                holder.Stats.AddStatsModifier(TargetParameter, -ActionValue);
            else holder.Actions.RemoveAction(ActivatorType, this);
        }

        public void Use(IArtifactUser holder)
        {
            executor.Use(holder);
            cooldownTimer = holder.SetTimer(Cooldown, t => RemainingCooldown = t, cooldownTimer);
        }


        public void SetModifier(ActorStatTypes type, int value, int seconds = 0)
        {
            if (seconds > 0) _internalModifiers.AddModifierTemp(type, value, seconds);
            else _internalModifiers.AddModifier(type, value);
        }
    }

    public class ArtifactPropertyExecutor
    {
        protected ArtifactProperty parentProperty;

        public ArtifactPropertyExecutor(ArtifactProperty parent)
        {
            parentProperty = parent;
        }

        public virtual void Init(IArtifactHolder holder) { }

        public virtual void Use(IArtifactUser holder)
        {
            UnityEngine.Debug.Log($"Executing Stub for {parentProperty.NameTag}");
        }
    }

    public class AttackExecutor : ArtifactPropertyExecutor
    {
        Ammo ammo;
        public AttackExecutor(ArtifactProperty parent) : base(parent) { }

        public override void Init(IArtifactHolder holder)
        {
            ammo = new(parentProperty.Ammo, holder.IsPlayer);
        }
        public override void Use(IArtifactUser holder)
        {
            Debug.Log($"Using attack");
            ammo.Start(holder.Barrel, holder.Direction, CalculateDamage(holder));
        }

        private int CalculateDamage(IArtifactUser holder) => holder.Stats.Damage + parentProperty.ActionValue;
    }
}