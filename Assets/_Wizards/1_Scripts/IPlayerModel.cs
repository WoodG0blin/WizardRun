using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IPlayerModel
    {
        List<IArtifact> EquippedArtifacts { get; }
        IArtifactExecutorsContainer Executors { get; }
        string Name { get; }

        bool TryEquipArtifact(IArtifact artifact);
        void RemoveArtifact(IArtifact artifact);
    }
}