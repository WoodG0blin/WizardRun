using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IPlayerModel
    {
        string Name { get; }

        CharacterStats Stats { get; }
        BonusStats Bonuses { get; }

        List<IArtifact> EquippedArtifacts { get; }
        List<ItemConfig> Chest { get; }
        //List<ArtifactActor> ArtifactActors { get; }
        //IArtifactExecutorsContainer Executors { get; }

        void EquipArtifact(ArtifactSlotType slot, ItemConfig artifact);
    }
}