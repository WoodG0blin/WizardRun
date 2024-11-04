using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace WizardsPlatformer
{
    internal abstract class Weapon : IWeapon
    {
        protected IWeaponConfig config;
        protected Transform barrel;
        protected Vector3 direction;

        public bool WeaponReady {get; private set;}

        public Weapon(IWeaponConfig config, Transform barrel)
        {
            this.config = config;
            this.barrel = barrel;

            WeaponReady = true;
            direction = Vector3.zero;
        }
        public void SetDirection(Vector3 direction) => this.direction = direction;

        public async void Fire()
        {
            if (WeaponReady)
            {
                WeaponReady = false;
                OnFire();
                await Task.Delay(Mathf.RoundToInt(config.CoolDown * 1000));
                WeaponReady = true;
            }
        }
        protected abstract void OnFire();


        public static IWeapon GetWeapon(Transform barrel, IWeaponConfig config)
        {
            if (!config.IsRanged) return new MeleeWeapon(config, barrel);
            else return new RangedWeapon(config, barrel, barrel.transform.parent.CompareTag("Player"));
        }
    }
}
