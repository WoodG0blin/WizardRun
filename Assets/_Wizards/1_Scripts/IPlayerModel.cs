using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IPlayerModel
    {
        List<IArtifact> EquippedArtifacts { get; }
        IArtifactExecutorsContainer Executors { get; }
        string Name { get; }

        void SetEquippedArtifacts(List<Artifact> artifacts);
    }
}