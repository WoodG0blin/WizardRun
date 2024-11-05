namespace WizardsPlatformer
{
    internal static class ArtifactExecutorFactory
    {
        public static ArtifactExecutor GetExecutor(string nameTag) => nameTag switch
        {
            _ => null
        };
    }

    internal static class ArtifactFactory
    {
        public static Artifact GetArtifact(ItemConfig config)
        {
            if(config is WeaponConfig wc)
            {
                if(wc.SlotType == ArtifactSlotType.Weapon) return GetWeapon(wc);
                else return GetActiveArtifact(wc);
            }
            return GetBasicArtifact(config);
        }

        private static Artifact GetBasicArtifact(ItemConfig config) => config.NameTag switch
        {
            _ => new Artifact(config)
        };

        private static Artifact GetActiveArtifact(WeaponConfig config) => config.NameTag switch
        {
            _ => new ActiveArtifact(config)
        };

        private static Artifact GetWeapon(WeaponConfig config) => config.NameTag switch
        {
            _ => new Weapon(config)
        };
    }
}
