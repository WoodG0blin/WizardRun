using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    public class Weapon : ActiveArtifact
    {

        protected GameObject ammoPrefab;
        protected AmmoView ammo;

        public Weapon(WeaponConfig config) : base(config)
        {
            ammoPrefab = config.Ammo; //register ammo in pool
        }

        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            if (ammo != null) RangedAttack(holder);
            else MeleeAttack(holder);
        }

        private void MeleeAttack(IArtifactHolder holder)
        {
            var hit = Physics2D.RaycastAll(holder.Barrel.position, holder.Direction, actionDistance.Value)
                    .Where(hit => hit.transform.CompareTag("Player"))
                    .FirstOrDefault();

            if (hit.collider != null)
            {
                (hit.transform.GetComponent<View>() as IDamagable)?.ReceiveDamage(damage.Value);
            }
        }

        private void RangedAttack(IArtifactHolder holder)
        {
            ammo = GameObject.Instantiate(ammoPrefab, holder.Barrel).GetComponent<AmmoView>();
            ammo.Init(holder.Barrel, damage.Value, fireForce.Value);
            if (holder.IsPlayer) ammo.ResetToPlayer();
            ammo.Fire(holder.Direction);
        }
    }
}
