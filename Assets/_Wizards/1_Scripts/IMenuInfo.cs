using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IMenuInfo
    {
        ISceneLoader SceneLoader { get; }
        IPlayerModel PlayerModel { get; }
        IReadOnlyList<ItemConfig> ArtifactDatabase { get; }
        void EquipArtifacts(List<Artifact> selectedArtifacts);
    }
}