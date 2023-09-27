using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class MeleeWeapon : Weapon
    {
        public MeleeWeapon(Transform barrel, float attackDistance, float damage, float coolodwn) : base(barrel, attackDistance, damage, coolodwn) { }

        protected override void OnFire()
        {
            var hit = Physics2D.RaycastAll(_barrel.position, _direction, _attackDistance)
                    .Where(hit => hit.transform.CompareTag("Player"))
                    .FirstOrDefault();

            if (hit.collider != null)
            {
                (hit.transform.GetComponent<View>() as IDamagable)?.ReceiveDamage(_damage);
            }
        }
    }
}
