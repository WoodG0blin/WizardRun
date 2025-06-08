using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel : IPlayerModel
    {
        public LevelObjectConfig Config {get; private set; }

        private BonusStats _bonuses;
        private Dictionary<ArtifactSlotType, Artifact> _artifacts;
        
        internal CharacterStats Stats { get; private set; }

        public string Name { get; private set; }
        public int Bonuses => _bonuses[BonusType.coin];
        public PlayerSavedData SaveData { get; private set; }

        public List<IArtifact> EquippedArtifacts => _artifacts.Values.Where(a => a!=null).Cast<IArtifact>().ToList();
        public IWeapon Weapon { get; protected set; }


        public PlayerModel(PlayerSavedData data, LevelObjectConfig config)
        {
            SaveData = data;

            Name = data.Name;

            Config = config;

            Stats = new(Config.MaxHealth, Config.Speed, Config.JumpForce);

            _bonuses = new BonusStats(false);
            _bonuses[BonusType.coin] = data.Bonuses;

            _artifacts = new()
            {
                { ArtifactSlotType.Weapon, null},
                { ArtifactSlotType.Head, null},
                { ArtifactSlotType.Neck, null},
                { ArtifactSlotType.Waist, null},
                { ArtifactSlotType.Legs, null}
            };
            
            Weapon initialWeapon = new Weapon(Config.MainWeaponConfig);
            _artifacts[ArtifactSlotType.Weapon] = initialWeapon;
            Weapon = initialWeapon;
        }


        public bool TrySetArtifactAt(ArtifactSlotType slot, ItemConfig artifact)
        {
            bool res =
                artifact != null ?
                slot == artifact.SlotType : true;
            // conditions to equip

            if (slot == ArtifactSlotType.Weapon)
            {
                var temp = artifact != null ? new Weapon(artifact) : null;
                _artifacts[slot] = temp;

                Weapon = temp;
                Weapon ??= new Weapon(Config.MainWeaponConfig);
            }
            else
            {
                _artifacts[slot] = new Artifact(artifact);
            }
            
            return res;
        }

        public void AddBonus(BonusType type, int value)
        {
            _bonuses[type] += value;
            SaveData.Bonuses = _bonuses[BonusType.coin];
        }

    }
}