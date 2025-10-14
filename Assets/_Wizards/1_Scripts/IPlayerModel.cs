using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IPlayerModel
    {
        string Name { get; }

        CharacterStats Stats { get; }
        BonusStats Bonuses { get; }

        List<IArtifact> EquippedArtifacts { get; }
        //List<ArtifactActor> ArtifactActors { get; }
        //IArtifactExecutorsContainer Executors { get; }


        bool TrySetArtifactAt(ArtifactSlotType slot, ItemConfig artifact);
    }
}