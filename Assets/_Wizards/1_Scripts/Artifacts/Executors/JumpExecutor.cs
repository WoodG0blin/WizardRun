namespace WizardsPlatformer
{
    internal class JumpExecutor : ArtifactExecutor
    {
        protected override void ActionsOnUse(IArtifactHolder holder)
        {
            UnityEngine.Debug.Log($"Extra actions on Jump");
            
            if(!holder.JumpExecutioner.IsGrounded) holder.JumpExecutioner.Jump(holder.Stats.JumpForce);
        }
    }
}
