using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

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

    public class ArtifactPropertySO : ScriptableObject
    {
        public ArtifactPropertyConfig Config;
        public AmmoConfig Ammo;

        public ArtifactPropertyConfig GetConfig()
        {
            var res = Config.Clone();
            res.SetAmmo(Ammo);
            return res;
        }
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
        [field: SerializeField] public string NameTag { get; set; }
        [field: SerializeField, HideInInspector] public string AmmoReference { get; set; }
        public AmmoConfig Ammo { get; set; }

        public void SetAmmo(AmmoConfig ammo)
        {
            Ammo = ammo;
            AmmoReference = Ammo != null ? Ammo.name : "";
        }

        public void LoadResources() =>
            SetAmmo(ArtifactDatabase.GetAmmoByName(AmmoReference));

        public ArtifactPropertyConfig Clone() => new()
        {
            ActivatorType = this.ActivatorType,
            ExecutionType = this.ExecutionType,
            TargetParameter = this.TargetParameter,
            ActionValue = this.ActionValue,
            CoolDown = this.CoolDown,
            NameTag = this.NameTag,
            Ammo = this.Ammo,
            AmmoReference = this.AmmoReference
        };
    }


    public interface IArtifactExecutor
    {
        void Use(IArtifactUser holder);
        bool IsReady { get; }
    }


    public class ArtifactProperty : IArtifactExecutor, IDisplayInfo
    {
        protected IArtifactHolder holder;

        protected ArtifactPropertyExecutor executor;

        protected int baseActionValue;
        public int ActionValue => baseActionValue;
        
        protected int baseCooldown;
        protected Coroutine cooldownTimer;
        public int Cooldown => baseCooldown;
        public float RemainingCooldown { get; protected set; }
        public bool IsReady => RemainingCooldown <= 0;


        public PropertyActivators ActivatorType { get; protected set; }
        public PropertyExecution ExecutionType { get; protected set; }
        public CharacterStatType TargetParameter { get; protected set; }
        public int ControlIndex { get; protected set; }
        
        public ArtifactPropertyConfig Config { get; protected set; }
        public string NameTag { get; protected set; }
        string IDisplayInfo.Name => NameTag;
        public Sprite Icon { get; private set; }
        public AmmoConfig Ammo => Config.Ammo;


        public ArtifactProperty(ArtifactPropertyConfig config, Artifact parentArt, int controlIndex = -1)
        {
            if (parentArt != null)
            {
                NameTag = parentArt.NameTag;
                Icon = parentArt.Icon;
            }

            Config = config;

            ActivatorType = config.ActivatorType;
            ExecutionType = config.ExecutionType;
            TargetParameter = config.TargetParameter;

            baseActionValue = config.ActionValue;

            baseCooldown = config.CoolDown;

            ControlIndex = controlIndex;

            executor = GetSpecificExecutor(config.NameTag);
            executor ??= ActivatorType switch
            {
                PropertyActivators.Explicit => new AttackExecutor(this),
                _ => new(this)
            };
            ControlIndex = controlIndex;
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
                holder.Stats.Modifiers.AddModifier(TargetParameter, ActionValue);
            else holder.Actions.AddAction(ActivatorType, this, ControlIndex >= 0);
        }

        public void DeInit()
        {
            if (ActivatorType == PropertyActivators.Passive)
                holder.Stats.Modifiers.AddModifier(TargetParameter, -ActionValue);
            else holder.Actions.RemoveAction(ActivatorType, this);
        }

        public void Use(IArtifactUser holder)
        {
            executor.Use(holder);
            cooldownTimer = holder.SetTimer(Cooldown, t => RemainingCooldown = t, cooldownTimer);
        }
    }
}