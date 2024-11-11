namespace WizardsPlatformer
{
    internal class JumpExecutor : ArtifactExecutor
    {
        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            UnityEngine.Debug.Log($"Extra actions on Jump");
        }
    }
}
