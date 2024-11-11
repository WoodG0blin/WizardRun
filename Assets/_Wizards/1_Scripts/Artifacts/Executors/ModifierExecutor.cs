using System.Linq;

namespace WizardsPlatformer
{
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
}
