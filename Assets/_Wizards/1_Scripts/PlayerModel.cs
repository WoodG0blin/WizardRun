using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel : IPlayerModel
    {
        private LevelObjectConfig _config;

        public BonusStats Bonuses { get; private set; }


        private Dictionary<ArtifactSlotType, IArtifact> _artifacts;

        public string Name { get; private set; }
        public PlayerSavedData SaveData { get; private set; }

        internal CharacterStats Stats { get; private set; }
        public LevelObjectConfig Config => _config;
        public IArtifactExecutorsContainer Executors => new ArtifactExecutorsContainer(EquippedArtifacts);

        public List<IArtifact> EquippedArtifacts => _artifacts.Values.Where(a => a!=null).ToList();


        public PlayerModel(PlayerSavedData data, LevelObjectConfig config)
        {
            SaveData = data;

            Name = data.Name;

            _config = config;

            Stats = new(_config.MaxHealth, _config.Speed, _config.JumpForce);

            Bonuses = new BonusStats(false);
            Bonuses[BonusType.coin] = data.Bonuses;

            _artifacts = new()
            {
                { ArtifactSlotType.Weapon, null},
                { ArtifactSlotType.Head, null},
                { ArtifactSlotType.Neck, null},
                { ArtifactSlotType.Waist, null},
                { ArtifactSlotType.Legs, null}
            };
            TrySetArtifactAt(ArtifactSlotType.Weapon, new Artifact(_config.WeaponConfig));
        }


        public bool TrySetArtifactAt(ArtifactSlotType slot, IArtifact artifact)
        {
            bool res =
                artifact != null ?
                slot == artifact.SlotType : true;
            // conditions to equip
            _artifacts[slot] = artifact;
            return res;
        }

        public void AddBonus(BonusType type, int value)
        {
            Bonuses[type] += value;
            SaveData.Bonuses = Bonuses[BonusType.coin];
        }

    }
}