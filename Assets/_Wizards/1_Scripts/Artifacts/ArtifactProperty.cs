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
        [field: SerializeField] public AmmoConfig Ammo { get; set; }
        [field: SerializeField] public string NameTag { get; set; }
    }

    public interface IArtifactExecutor
    {
        void Use(IArtifactUser holder);
        bool IsReady { get; }
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

            executor = GetSpecificExecutor(config.NameTag);
            executor ??= ActivatorType switch
            {
                PropertyActivators.Explicit => new AttackExecutor(this),
                _ => new(this)
            };
        }

        private ArtifactPropertyExecutor GetSpecificExecutor(string tag) => tag switch
        {
            string a when a.Contains("ExtraJump") => new ExtraJumpExecutor(this),
            string a when a.Contains("ExtraShot") => new ExtraShotExecutor(this),
            _ => null
        };

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
}