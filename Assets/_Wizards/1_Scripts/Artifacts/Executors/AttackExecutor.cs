using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class AttackExecutor : ArtifactExecutor
    {
        protected new Weapon parentArtifact;
        AmmoView ammo;

        public override void Init(Artifact parentArtifact)
        {
            //this.parentArtifact = parentArtifact as Weapon;
        }

        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            //if (parentArtifact == null) return;

            //if (parentArtifact.IsRanged) RangedAttack(holder);
            //else MeleeAttack(holder);
        }

        private void MeleeAttack(IArtifactHolder holder)
        {
            //var hits = Physics.RaycastAll(holder.Barrel.position, holder.Direction, parentArtifact.ActionDistance)
            //    .Select(h => h.transform.GetComponent<LevelObjectView>());

            //IInteractionResponder hit = null;
            
            //foreach (var h in hits)
            //{
            //    if (h != null && h.InteractionResponder != null)
            //    {
            //        hit = h.InteractionResponder;
            //        break;
            //    }
            //}

            //if (hit == null) return;

            //if (holder.IsPlayer ^ hit.IsPlayer) hit.ReceiveDamage(parentArtifact.Damage);
        }

        private void RangedAttack(IArtifactHolder holder)
        {
            //parentArtifact.TryGetAmmoTo(holder.Barrel, out ammo);
            ////ammo.Init(holder.Barrel, parentArtifact.Damage, parentArtifact.FireForce);
            ////if (holder.IsPlayer) ammo.ResetToPlayer();
            //ammo.SetMove(holder.Direction);
        }
    }
}
