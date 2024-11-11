using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifactExecutor
    {
        void Use(IArtifactHolder holder);
    }

    internal abstract class ArtifactExecutor : IArtifactExecutor
    {
        protected IArtifactExecutorHolder parentArtifact;

        public void Init(IArtifactExecutorHolder holder) => parentArtifact = holder;

        public void Use(IArtifactHolder holder)
        {
            if (parentArtifact.IsReady)
            {
                parentArtifact.Activate();
                ActionsOnUse(holder);
            }
        }

        protected abstract void ActionsOnUse(IArtifactHolder holder);
    }

    internal class ModifierExecutor : ArtifactExecutor
    {
        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            var targets = holder.EquippedArtifacts.Where(Condition).Cast<IModifiableArtifact>().ToList();
            if (targets != null)
                foreach (var target in targets)
                    Modify(target);
        }

        protected virtual bool Condition(IArtifact art) => art is IModifiableArtifact;
        protected virtual void Modify(IModifiableArtifact target)
        {
            UnityEngine.Debug.Log($"Modifying {(target as IArtifact).Name} with {(parentArtifact as IArtifact).Name}");
        }
    }

    internal class JumpExecutor : ArtifactExecutor
    {
        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            UnityEngine.Debug.Log($"Extra actions on Jump");
        }
    }

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
            var hit = Physics2D.RaycastAll(holder.Barrel.position, holder.Direction, parentArtifact.ActionDistance)
                    .Where(hit => hit.transform.CompareTag("Player"))
                    .FirstOrDefault();

            if (hit.collider != null)
            {
                (hit.transform.GetComponent<LevelObjectView>() as IDamagable)?.ReceiveDamage(parentArtifact.Damage);
            }
        }

        private void RangedAttack(IArtifactHolder holder)
        {
            if (holder.IsPlayer) ammo.ResetToPlayer();
            ammo.Fire(holder.Direction);
        }
    }

    internal class StubExecutor : ArtifactExecutor
    {
        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            UnityEngine.Debug.Log($"Executing Stub for {(parentArtifact as IArtifact).Name}");
        }
    }
}
