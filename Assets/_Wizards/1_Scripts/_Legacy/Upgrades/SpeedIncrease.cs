using Unity.Mathematics;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IStatsUpgrade : IUpgrade
    {
        public void Init(Stats stats);
    }
    internal class SpeedIncrease : Upgrade, IStatsUpgrade
    {
        private Stats _stats;
        private bool activated = false;
        public SpeedIncrease(UpgradeConfig config) : base(config) { activated = false; }
        public void Init(Stats stats) => _stats = stats;
        protected override void OnActivation()
        {
            if (!activated)
            {
                _stats.Speed += Config.Value;
                activated = true;
            }
        }
    }
}