using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace WizardsPlatformer
{
    internal abstract class Weapon : IWeapon
    {
        protected Transform _barrel;

        protected bool _weaponReady;
        protected Vector3 _direction;

        protected float _attackDistance;
        protected float _damage;

        private float _coolDown;
        public bool WeaponReady {get => _weaponReady;}

        public Weapon(Transform barrel, float attackDistance, float damage, float cooldown)
        {
            _barrel = barrel;

            _weaponReady = true;
            _direction = Vector3.zero;

            _attackDistance = attackDistance;
            _damage = damage;
            _coolDown = cooldown;
        }

        public async void Fire()
        {
            if (_weaponReady)
            {
                _weaponReady = false;
                OnFire();
                await Task.Delay(Mathf.RoundToInt(_coolDown * 1000));
                _weaponReady = true;
            }
        }

        public void SetDirection(Vector3 direction) => _direction = direction;

        protected abstract void OnFire();

        public static IWeapon GetWeapon(Transform barrel, IWeaponConfig config)
        {
            if (!config.IsRanged) return new MeleeWeapon(barrel, config.AttackDistance, config.Damage, config.CoolDown);
            else return new RangedWeapon(barrel, GameObject.Instantiate(config.AmmoPrefab, barrel.transform.parent).GetComponent<AmmoView>(),
                config.AttackDistance, config.Damage, config.FireForce, config.CoolDown, barrel.transform.parent.CompareTag("Player"));
        }
    }
}
