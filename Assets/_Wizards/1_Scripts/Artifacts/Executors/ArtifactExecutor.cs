using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifactExecutor
    {
        void Use(IArtifactUser holder);
        bool IsReady { get; }
    }

    //public abstract class ArtifactExecutor : IArtifactExecutor
    //{
    //    protected ArtifactActor parentArtifact;
    //    public bool IsReady => true;

    //    public virtual void Init(ArtifactActor parentArtifact) => this.parentArtifact = parentArtifact;

    //    public void Use(IArtifactUser holder)
    //    {
    //        ActionsOnUse(holder);
    //    }

    //    protected abstract void ActionsOnUse(IArtifactUser holder);
    //}

    //internal class StubExecutor : ArtifactExecutor
    //{
    //    protected override void ActionsOnUse(IArtifactUser holder)
    //    {
    //        UnityEngine.Debug.Log($"Executing Stub for {(parentArtifact as IArtifact).Name}");
    //    }
    //}
}
