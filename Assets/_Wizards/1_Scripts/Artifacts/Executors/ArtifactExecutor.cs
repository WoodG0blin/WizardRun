using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifactExecutor
    {
        bool IsReady { get; }
        void Use(IArtifactHolder holder);
    }

    internal abstract class ArtifactExecutor : IArtifactExecutor
    {
        protected Artifact parentArtifact;
        protected Coroutine cooldownTimer;

        public bool IsReady => RemainingCooldown <= 0;
        public float RemainingCooldown { get; protected set; }

        public virtual void Init(Artifact parentArtifact) => this.parentArtifact = parentArtifact;

        public void Use(IArtifactHolder holder)
        {
            cooldownTimer = holder.SetTimer(parentArtifact.Cooldown, t => RemainingCooldown = t, cooldownTimer);
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
