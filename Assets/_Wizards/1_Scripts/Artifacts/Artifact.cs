using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardsPlatformer
{
    public class Artifact : IArtifact
    {
        private ItemConfig _config;
        //private ArtifactExecutor _executor;

        protected Stat<ArtifactStatTypes> cooldown;
        private bool _forceResetCooldown;

        public Artifact(ItemConfig config)
        {
            _config = config;
            //_executor = ArtifactExecutorFactory.GetExecutor(_config.NameTag);

            cooldown = new(ArtifactStatTypes.Cooldown, _config.Cooldown);

            PassiveCharacterModifiers = _config.PassiveCharacterModifiers;
        }


        public string Name => _config.NameTag; //replace with localization
        public Sprite Icon => _config.Icon;
        public Sprite LevelView => _config.LevelView;
        public ArtifactSlotType SlotType => _config.SlotType;

        public bool HasPassiveArtifactModifier => _config.HasPassiveArtifactModifier;
        public bool IsActive => _config.HasActiveExecutor;

        public bool IsReady => RemainingCooldown <= 0;
        public int RemainingCooldown { get; private set; }

        public List<ICharacterModifier> PassiveCharacterModifiers { get; private set; }


        //public void Use(IArtifactHolder holder) => _executor.Use(holder);

        public void Use(IArtifactHolder holder)
        {
            if (IsReady)
            {
                ActionsOnUse(holder);

                RemainingCooldown = cooldown.Value;
                StartCooldownArtifact();
            }
        }
        protected virtual void ActionsOnUse(IArtifactHolder holder) { }

        public void UseToModify() { }
        
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
    }
}
