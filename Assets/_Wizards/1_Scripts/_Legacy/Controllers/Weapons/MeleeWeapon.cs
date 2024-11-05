using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class MeleeWeapon : WeaponLegacy
    {
        public MeleeWeapon(WeaponConfig config, Transform barrel) : base(config, barrel) { }

        protected override void OnFire()
        {
            var hit = Physics2D.RaycastAll(barrel.position, direction, config.ActionDistance)
                    .Where(hit => hit.transform.CompareTag("Player"))
                    .FirstOrDefault();

            if (hit.collider != null)
            {
                (hit.transform.GetComponent<View>() as IDamagable)?.ReceiveDamage(config.Damage);
            }
        }
    }
}
