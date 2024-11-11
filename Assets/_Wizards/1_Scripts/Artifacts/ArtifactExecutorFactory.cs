namespace WizardsPlatformer
{
    internal static class ArtifactExecutorFactory
    {
        public static ArtifactExecutor GetExecutor(ArtifactExecutorType type, string nameTag) => type switch
        {
            ArtifactExecutorType.Modifier => GetModifierExecutor(nameTag),
            ArtifactExecutorType.Jump => GetJumpExecutor(nameTag),
            ArtifactExecutorType.Attack => GetAttackExecutor(nameTag),
            _ => GetActionExecutor(nameTag)
        };

        private static ArtifactExecutor GetModifierExecutor(string nameTag) => nameTag switch
        {
            _ => new ModifierExecutor()
        };
        private static ArtifactExecutor GetJumpExecutor(string nameTag) => nameTag switch
        {
            _ => new JumpExecutor()
        };
        private static ArtifactExecutor GetAttackExecutor(string nameTag) => nameTag switch
        {
            _ => new AttackExecutor()
        };
        private static ArtifactExecutor GetActionExecutor(string nameTag) => nameTag switch
        {
            _ => new StubExecutor()
        };
    }
}
