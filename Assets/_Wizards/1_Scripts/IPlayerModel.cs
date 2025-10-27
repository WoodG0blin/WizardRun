using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IPlayerModel
    {
        string Name { get; }
        Sprite Icon { get; set; }
        void SetNewDisplayName(string newName);
        MasteryData Mastery { get; }

        LevelObjectConfig Config { get; }
        CharacterStats Stats { get; }
        Dictionary<BonusType, int> Bonuses { get; }

        List<IArtifact> EquippedArtifacts { get; }
        List<ItemConfig> Chest { get; }
        int MaxInventorySlots { get; }
        ActionsHolder Actions { get; }

        IReadOnlyList<RewardData> CollectedRewards { get; }
        DateTime LastEntryDate { get; }
        DateTime CurrentEntryDate { get; }

        Action OnValuesChanged { get; set; }
        //List<ArtifactActor> ArtifactActors { get; }
        //IArtifactExecutorsContainer Executors { get; }

        void EquipArtifact(ArtifactSlotType slot, ItemConfig artifact);
        void AddArtifact(ItemConfig artifact);
        void AddBonus(BonusType type, int value);
        void AccountReward(RewardData reward);
    }
}