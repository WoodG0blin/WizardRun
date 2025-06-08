using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class AttackExecutor : ArtifactExecutor
    {
        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            if (parentArtifact == null) return;

            if (parentArtifact.IsRanged) RangedAttack(holder);
            else MeleeAttack(holder);
        }

        private void MeleeAttack(IArtifactHolder holder)
        {
            IInteractionResponder hit = holder.GetTargetAt(parentArtifact.ActionDistance);

            if (hit == null) return;

            if (holder.IsPlayer ^ hit.IsPlayer) hit.ReceiveDamage(parentArtifact.ActionValue);
        }

        private void RangedAttack(IArtifactHolder holder)
        {
            Ammo bullet = new Ammo(parentArtifact.ActionValue, prefab: parentArtifact.Ammo);
            holder.PlaceAmmo(bullet);
            bullet.SetToPlayer(holder.IsPlayer);
            bullet.Fire(holder.Direction, parentArtifact.ActionSpeed);
        }
    }
}
