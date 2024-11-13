using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel : IPlayerModel
    {
        private LevelObjectConfig _config;

        private Dictionary<ArtifactSlotType, IArtifact> _artifacts;

        public string Name { get; private set; }
        internal CharacterStats Stats { get; private set; }
        public LevelObjectConfig Config => _config;
        public IArtifactExecutorsContainer Executors => new ArtifactExecutorsContainer(EquippedArtifacts);

        public List<IArtifact> EquippedArtifacts => _artifacts.Values.Where(a => a!=null).ToList();


        public PlayerModel()
        {
            _artifacts = new()
            {
                { ArtifactSlotType.Weapon, null},
                { ArtifactSlotType.Head, null},
                { ArtifactSlotType.Neck, null},
                { ArtifactSlotType.Waist, null},
                { ArtifactSlotType.Legs, null}
            };
        }

        public void SetConfig(LevelObjectConfig config)
        {
            _config = config;

            Stats = new(_config.MaxHealth, _config.Speed);
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
    }
}