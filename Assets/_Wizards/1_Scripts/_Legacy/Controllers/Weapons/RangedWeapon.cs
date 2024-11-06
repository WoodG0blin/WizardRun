using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class RangedWeapon : WeaponLegacy
    {
        private AmmoView _ammo;
        public RangedWeapon(WeaponConfig config, Transform barrel, bool isFromPlayer = false) : base(config, barrel)
        {
            _ammo = GameObject.Instantiate(config.Ammo, barrel).GetComponent<AmmoView>();
            _ammo.Init(barrel, config.Damage, config.FireForce);
            if (isFromPlayer) _ammo.ResetToPlayer();
        }

        public AmmoView Ammo { get => _ammo; }
        
        protected override void OnFire()
        {
            if (_ammo != null) _ammo.Fire(direction);
        }
    }
}
