using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IPlayerModel
    {
        List<IArtifact> EquippedArtifacts { get; }
        List<ArtifactActor> ArtifactActors { get; }
        //IArtifactExecutorsContainer Executors { get; }

        string Name { get; }
        int Bonuses { get; }

        bool TrySetArtifactAt(ArtifactSlotType slot, ItemConfig artifact);
    }
}