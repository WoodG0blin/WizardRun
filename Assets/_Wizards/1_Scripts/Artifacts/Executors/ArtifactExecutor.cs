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

    internal class StubExecutor : ArtifactExecutor
    {
        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            UnityEngine.Debug.Log($"Executing Stub for {(parentArtifact as IArtifact).Name}");
        }
    }
}
