using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class UpgradesManager : Controller
    {
        private UpgradeRepository _upgradeRepository;

        public UpgradesManager(IEnumerable<UpgradeConfig> configs)
        {
            _upgradeRepository= new UpgradeRepository(configs);
            Register(_upgradeRepository);
        }

        public void SetUpgrades(IUpgradable target)
        {
            target.Reset();

            foreach (Upgrade upgrade in _upgradeRepository.Items.Values)
            {
                if(upgrade is IStatsUpgrade su)
                    {
                        su.Init(target.Stats);
                        target.Upgrades[ActivatorType.OnStats].Add(su);
                        break;
                    }
                else if(upgrade is IJumpUpgrade ju)
                    {
                        ju.Init(target.Jumper);
                        target.Upgrades[ActivatorType.OnJump].Add(ju);
                        break;
                    }
            }
        }
    }
}