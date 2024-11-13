using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class AttackExecutor : ArtifactExecutor
    {
        AmmoView ammo;

        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            if (parentArtifact.TryGetAmmoTo(holder.Barrel, out ammo))
            {
                ammo.Init(holder.Barrel, parentArtifact.Damage, parentArtifact.FireForce);
                RangedAttack(holder);
            }
            else MeleeAttack(holder);
        }

        private void MeleeAttack(IArtifactHolder holder)
        {
            //var hit = Physics2D.RaycastAll(holder.Barrel.position, holder.Direction, parentArtifact.ActionDistance)
            //        .Where(hit => hit.transform.CompareTag("Player"))
            //        .FirstOrDefault();

            var hits = Physics.RaycastAll(holder.Barrel.position, holder.Direction, parentArtifact.ActionDistance)
                .Select(h => h.transform.GetComponent<LevelObjectView>());

            IInteractionResponder hit = null;
            
            foreach (var h in hits)
            {
                if (h != null && h.InteractionResponder != null)
                {
                    hit = h.InteractionResponder;
                    break;
                }
            }

            if (hit == null) return;

            if (holder.IsPlayer ^ hit.IsPlayer) hit.ReceiveDamage(parentArtifact.Damage);
        }

        private void RangedAttack(IArtifactHolder holder)
        {
            if (holder.IsPlayer) ammo.ResetToPlayer();
            ammo.Fire(holder.Direction);
        }
    }
}
