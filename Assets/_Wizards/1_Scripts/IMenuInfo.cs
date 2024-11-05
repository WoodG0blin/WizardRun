using System.Collections.Generic;

namespace WizardsPlatformer
{
    interface IMenuInfo
    {
        ISceneLoader SceneLoader { get; }
        IReadOnlyList<ItemConfig> ArtifactDatabase { get; }
        void EquipArtifacts(List<Artifact> selectedArtifacts);
    }
}