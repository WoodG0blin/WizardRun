using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class UpgradeFactory
    {
        public Upgrade GetUpgrade(UpgradeConfig config)
        {
            switch(config.Upgrade)
            {
                case UpgradeType.SpeedIncrease: return new SpeedIncrease(config);
                case UpgradeType.IncreasedJump: return new IncreasedJump(config);
                case UpgradeType.BasicAttack: return StubUpgrade.Default;
                case UpgradeType.BasicDefence: return StubUpgrade.Default;
                case UpgradeType.MultipleJump: return new MultipleJump(config);
                default: return StubUpgrade.Default;

            }
        }
    }
}