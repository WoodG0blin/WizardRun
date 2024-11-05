using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class IncreasedJump : Upgrade, IStatsUpgrade
    {
        private CharacterStats _stats;
        private bool activated = false;

        public IncreasedJump(UpgradeConfig config) : base(config) { activated = false; }
        public void Init(CharacterStats stats) => _stats = stats;
        protected override void OnActivation()
        {
            if (!activated)
            {
                _stats.AddStatsModifier(CharacterStatType.JumpForce, Config.Value);
                activated = true;
            }
        }
    }
}