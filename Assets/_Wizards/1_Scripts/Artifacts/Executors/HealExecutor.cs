namespace WizardsPlatformer
{
    public class HealExecutor : ArtifactPropertyExecutor
    {
        public HealExecutor(ArtifactProperty parent) : base(parent) { }

        public override void Use(IArtifactUser holder)
        {
            holder.ReceiveDamage(-parentProperty.ActionValue);
        }
    }
}