using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel
    {
        private LevelObjectConfig _config;
        private List<UpgradeConfig> _upgrades;
        private WeaponConfig _weaponConfig;

        public string Name { get; private set; }
        public int MaxHealth {get; private set; }
        public int Speed { get; private set; }


        public IReadOnlyList<UpgradeConfig> Upgrades => _upgrades;
        public GameObject Prefab => _config.Prefab;
        public AnimationSequence[] Animations => _config.Animations;
        public IWeapon GetWeaponTo(Transform barrel) => WeaponLegacy.GetWeapon(barrel, _weaponConfig);


        public PlayerModel()
        {
            _upgrades = new List<UpgradeConfig>();
            MaxHealth = 300;
        }

        public void SetConfig(LevelObjectConfig config)
        {
            _config = config;
            _weaponConfig = config.WeaponConfig;
        }

        public void AddUpgrade(UpgradeConfig upgrade) => _upgrades.Add(upgrade);
        public void Reset() => _upgrades.Clear();
    }
}