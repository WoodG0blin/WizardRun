using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class UpgradeRepository : Repository<string, Upgrade, UpgradeConfig>
    {
        private UpgradeFactory _upgradeFactory = new UpgradeFactory();
        public UpgradeRepository(IEnumerable<UpgradeConfig> configs) : base(configs) {}

        protected override Upgrade CreateItem(UpgradeConfig config) => _upgradeFactory.GetUpgrade(config);

        protected override string GetKey(UpgradeConfig config) => config.Description;
    }
}