using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace WizardsPlatformer
{
    internal abstract class WeaponLegacy : IWeapon
    {
        protected WeaponConfig config;
        protected Transform barrel;
        protected Vector3 direction;

        public bool WeaponReady {get; private set;}

        public WeaponLegacy(WeaponConfig config, Transform barrel)
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
                await Task.Delay(Mathf.RoundToInt(5 * 1000));
                WeaponReady = true;
            }
        }
        protected abstract void OnFire();


        public static IWeapon GetWeapon(Transform barrel, WeaponConfig config)
        {
            return new MeleeWeapon(config, barrel);
        }
    }
}
