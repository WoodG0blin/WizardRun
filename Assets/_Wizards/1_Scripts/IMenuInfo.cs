using PlayFab.ClientModels;
using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IMenuInfo
    {
        ISceneLoader SceneLoader { get; }
        IPlayerModel PlayerModel { get; }
        IReadOnlyList<ItemSO> ArtifactDatabase { get; }

        PLayFabController PlayFabController { get; }
        SoundManager SoundManager { get; }

        void StartForNewPlayer();
        void QuitGame();
        void SaveGame();

        List<Location> Locations { get; }

        void SetActiveLocation(Location location);
    }
}