using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IMenuInfo
    {
        ISceneLoader SceneLoader { get; }
        IPlayerModel PlayerModel { get; }
        IReadOnlyList<ItemConfig> ArtifactDatabase { get; }

        void RegisterNewPlayer(string name);
        void SaveGame();

        List<Location> Locations { get; }

        void SetActiveLocation(Location location);
    }
}