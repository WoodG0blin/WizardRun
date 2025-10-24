using System.Collections.Generic;

namespace WizardsPlatformer
{
    public interface IPlayerModel
    {
        string Name { get; }
        void SetNewDisplayName(string newName);
        MasteryData Mastery { get; }

        LevelObjectConfig Config { get; }
        CharacterStats Stats { get; }
        Dictionary<BonusType, int> Bonuses { get; }

        List<IArtifact> EquippedArtifacts { get; }
        List<ItemConfig> Chest { get; }
        int MaxInventorySlots { get; }
        ActionsHolder Actions { get; }
        //List<ArtifactActor> ArtifactActors { get; }
        //IArtifactExecutorsContainer Executors { get; }

        void EquipArtifact(ArtifactSlotType slot, ItemConfig artifact);
    }
}