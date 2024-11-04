using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class RangedWeapon : Weapon
    {
        private AmmoView _ammo;
        public RangedWeapon(IWeaponConfig config, Transform barrel, bool isFromPlayer = false) : base(config, barrel)
        {
            _ammo = GameObject.Instantiate(config.AmmoPrefab, barrel).GetComponent<AmmoView>();
            _ammo.Init(barrel, config.Damage, config.FireForce);
            if (isFromPlayer) _ammo.ResetToPlayer();
        }

        public AmmoView Ammo { get => _ammo; }
        
        protected override void OnFire()
        {
            if (_ammo != null && _ammo.Ready) _ammo.Fire(direction);
        }
    }
}
