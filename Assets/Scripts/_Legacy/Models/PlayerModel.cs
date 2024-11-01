using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel
    {
        private LevelObjectConfig _config;
        private List<UpgradeConfig> _upgrades;
        private IWeaponConfig _weaponConfig;

        public string Name { get; private set; }
        public float MaxHealth {get; private set; }
        public float Speed { get; private set; }

        public int Bonuses { get; private set; }


        public IReadOnlyList<UpgradeConfig> Upgrades => _upgrades;
        public GameObject Prefab => _config.Prefab;
        public AnimationSequence[] Animations => _config.Animations;
        public IWeapon GetWeaponTo(Transform barrel) => Weapon.GetWeapon(barrel, _weaponConfig);



        public PlayerModel(LevelModel levelModel) { }
        public PlayerModel()
        {
            _upgrades = new List<UpgradeConfig>();
        }

        public void SetConfig(LevelObjectConfig config)
        {
            _config = config;
            _weaponConfig = config.WeaponConfig;
        }

        public void AddUpgrade(UpgradeConfig upgrade) => _upgrades.Add(upgrade);
        public void Reset() => _upgrades.Clear();
        public void AddBonuses(int value) => Bonuses += value;
    }
}