using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.WSA;

namespace WizardsPlatformer
{
    public class Artifact : IArtifact, IArtifactExecutorHolder, IModifiableArtifact
    {
        private ItemConfig _config;

        protected Stat<ArtifactStatTypes> cooldown;
        private bool _forceResetCooldown;

        protected Dictionary<ArtifactExecutorType, IArtifactExecutor> executors;

        protected Stat<ArtifactStatTypes> damage;
        protected Stat<ArtifactStatTypes> actionDistance;
        protected Stat<ArtifactStatTypes> fireForce;


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

            cooldown = new(ArtifactStatTypes.Cooldown, _config.Cooldown);

            PassiveCharacterModifiers = _config.PassiveCharacterModifiers;

            damage = new(ArtifactStatTypes.Damage, config.Damage);
            actionDistance = new(ArtifactStatTypes.ActionDistance, config.ActionDistance);
            fireForce = new(ArtifactStatTypes.FireForce, config.FireForce);
        }


        public string Name => _config.NameTag; //replace with localization
        public Sprite Icon => _config.Icon;
        public Sprite LevelView => _config.LevelView;

        public ArtifactSlotType SlotType => _config.SlotType;

        public List<ICharacterModifier> PassiveCharacterModifiers { get; private set; }

        public bool IsReady => RemainingCooldown <= 0;
        public int RemainingCooldown { get; private set; }


        int IArtifactExecutorHolder.Damage => damage.Value;
        int IArtifactExecutorHolder.ActionDistance => actionDistance.Value;
        int IArtifactExecutorHolder.FireForce => fireForce.Value;
        bool IArtifactExecutorHolder.TryGetAmmoTo(Transform barrel, out AmmoView ammo)
        {
            ammo = null;

            if(_config.Ammo != null)
            {
                ammo = GameObject.Instantiate(_config.Ammo, barrel).GetComponent<AmmoView>();
                ammo.Init(barrel, damage.Value, fireForce.Value);
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
                RemainingCooldown = cooldown.Value;
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
