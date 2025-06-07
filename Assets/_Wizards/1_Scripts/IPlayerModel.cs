using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IPlayerModel
    {
        List<IArtifact> EquippedArtifacts { get; }
        //IArtifactExecutorsContainer Executors { get; }

        string Name { get; }
        int Bonuses { get; }

        bool TrySetArtifactAt(ArtifactSlotType slot, IArtifact artifact);
    }
}