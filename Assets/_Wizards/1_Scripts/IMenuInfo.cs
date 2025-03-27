using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IMenuInfo
    {
        ISceneLoader SceneLoader { get; }
        IPlayerModel PlayerModel { get; }
        int Bonuses { get; }
        int Score { get; }
        List<Location> Locations { get; }
        IReadOnlyList<ItemConfig> ArtifactDatabase { get; }
        bool Loaded { get; }
        void ApplyData(PlayerSavedData data);
        PlayerSavedData GetData();
    }
}