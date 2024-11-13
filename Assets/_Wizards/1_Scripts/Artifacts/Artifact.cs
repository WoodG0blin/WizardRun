using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardsPlatformer
{
    public class Artifact : IArtifact, IArtifactExecutorHolder, IModifiableArtifact
    {
        private ItemConfig _config;

        protected int cooldown;
        private bool _forceResetCooldown;

        protected Dictionary<ArtifactExecutorType, IArtifactExecutor> executors;

        protected int damage;
        protected float actionDistance;
        protected float fireForce;

        private ParametersModifier<ArtifactStatTypes> _modifiers;

        public Artifact(ItemConfig config)
        {
            _config = config;
            executors = new();
            foreach (var type in _config.ArtifactExecutors)
            {
                var executor = ArtifactExecutorFactory.GetExecutor(type, _config.NameTag);
                executor.Init(this);
                executors.Add(type, executor);
            }

            cooldown = _config.Cooldown;

            PassiveCharacterModifiers = _config.PassiveCharacterModifiers;

            damage = config.Damage;
            actionDistance = config.ActionDistance;
            fireForce = config.FireForce;

            _modifiers = new();
        }


        public string Name => _config.NameTag; //replace with localization
        public Sprite Icon => _config.Icon;
        public Sprite LevelView => _config.LevelView;

        public ArtifactSlotType SlotType => _config.SlotType;

        public List<ICharacterModifier> PassiveCharacterModifiers { get; private set; }

        public bool IsReady => RemainingCooldown <= 0;
        public int RemainingCooldown { get; private set; }


        int IArtifactExecutorHolder.Damage => damage + _modifiers.GetModifier(ArtifactStatTypes.Damage);
        float IArtifactExecutorHolder.ActionDistance => actionDistance + _modifiers.GetModifier(ArtifactStatTypes.ActionDistance);
        float IArtifactExecutorHolder.FireForce => fireForce + _modifiers.GetModifier(ArtifactStatTypes.FireForce);
        bool IArtifactExecutorHolder.TryGetAmmoTo(Transform barrel, out AmmoView ammo)
        {
            ammo = null;

            if(_config.Ammo != null)
            {
                var temp = GameObject.Instantiate(_config.Ammo, barrel);
                if(!temp.TryGetComponent<AmmoView>(out ammo)) ammo = temp.AddComponent<BulletView>();
                
                return true;
            }

            return false;
        }


        public IArtifactExecutor GetExecutor(ArtifactExecutorType type)
        {
            if (executors.ContainsKey(type)) return executors[type];
            else return null;
        }


        public void Activate()
        {
            if (IsReady)
            {
                RemainingCooldown = cooldown;
                StartCooldownArtifact();
            }
        }
        public void ResetCooldown()
        {
            RemainingCooldown = 0;
            _forceResetCooldown = true;
        }
        private async void StartCooldownArtifact()
        {
            if (RemainingCooldown > 0)
            {
                _forceResetCooldown = false;
                await Task.Delay(1000);
                if (!_forceResetCooldown) RemainingCooldown--;
                StartCooldownArtifact();
            }
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
