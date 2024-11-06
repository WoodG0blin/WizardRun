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

    //internal static class ArtifactFactory
    //{
    //    public static Artifact GetArtifact(ItemConfig config)
    //    {
    //        if(config is WeaponConfig wc)
    //        {
    //            if(wc.SlotType == ArtifactSlotType.Weapon) return GetWeapon(wc);
    //            else return GetActiveArtifact(wc);
    //        }
    //        return GetBasicArtifact(config);
    //    }

    //    private static Artifact GetBasicArtifact(ItemConfig config) => config.NameTag switch
    //    {
    //        _ => new Artifact(config)
    //    };

    //    private static Artifact GetActiveArtifact(WeaponConfig config) => config.NameTag switch
    //    {
    //        _ => new ActiveArtifact(config)
    //    };

    //    private static Artifact GetWeapon(WeaponConfig config) => config.NameTag switch
    //    {
    //        _ => new Weapon(config)
    //    };
    //}
}
