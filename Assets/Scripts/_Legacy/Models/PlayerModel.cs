using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel : Model
    {
        private List<UpgradeConfig> _upgrades = new List<UpgradeConfig>();
        public IReadOnlyList<UpgradeConfig> Upgrades => _upgrades;
        public LevelModel LevelModel { get; private set; }
        public PlayerModel(LevelModel levelModel) => LevelModel = levelModel;

        public void AddUpgrade(UpgradeConfig upgrade)
        {
            _upgrades ??= new List<UpgradeConfig>();
            _upgrades.Add(upgrade);
        }

        public void Reset()
        {
            _upgrades.Clear();
        }
    }
}