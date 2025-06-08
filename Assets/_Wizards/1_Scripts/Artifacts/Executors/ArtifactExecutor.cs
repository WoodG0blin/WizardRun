using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifactExecutor
    {
        void Use(IArtifactHolder holder);
    }

    internal abstract class ArtifactExecutor : IArtifactExecutor
    {
        protected ArtifactActor parentArtifact;


        public virtual void Init(ArtifactActor parentArtifact) => this.parentArtifact = parentArtifact;

        public void Use(IArtifactHolder holder)
        {
            ActionsOnUse(holder);
        }

        protected abstract void ActionsOnUse(IArtifactHolder holder);
    }

    internal class StubExecutor : ArtifactExecutor
    {
        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            UnityEngine.Debug.Log($"Executing Stub for {(parentArtifact as IArtifact).Name}");
        }
    }
}
