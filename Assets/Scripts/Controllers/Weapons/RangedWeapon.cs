using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class RangedWeapon : Weapon
    {
        private AmmoView _ammo;
        public RangedWeapon(Transform barrel, AmmoView bullet, float attackDistance, float damage, float speed, float coolodwn, bool isFromPlayer = false) : base(barrel, attackDistance, damage, coolodwn)
        {
            _ammo = bullet;
            _ammo.Init(barrel, damage, speed);
            if (isFromPlayer) _ammo.ResetToPlayer();

            //var ammoGO = weaponView.transform.Find("Ammo").gameObject;
            //if (ammoGO != null)
            //{
            //    if(!ammoGO.TryGetComponent<BulletView>(out _ammo)) _ammo = ammoGO.AddComponent<BulletView>();
            //    _ammo.Init(weaponView.transform, damage, speed);
            //    if(isFromPlayer) _ammo.ResetToPlayer();
            //}
        }

        public AmmoView Ammo { get => _ammo; }
        
        protected override void OnFire()
        {
            if (_ammo != null && _ammo.Ready) _ammo.Fire(_direction);
        }
    }
}
