namespace WizardsPlatformer
{
    internal static class ArtifactExecutorFactory
    {
        public static ArtifactExecutor GetExecutor(string nameTag) => nameTag switch
        {
            _ => null
        };
    }
}
