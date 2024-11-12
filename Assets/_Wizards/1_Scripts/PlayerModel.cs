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
        public IArtifactExecutorsContainer Executors => new ArtifactExecutorsContainer(_artifacts.Values.ToList());

        public List<IArtifact> EquippedArtifacts => _artifacts.Values.ToList();


        public PlayerModel()
        {
            _artifacts = new();
        }

        public void SetConfig(LevelObjectConfig config)
        {
            _config = config;

            Stats = new(_config.MaxHealth, _config.Speed);
            _artifacts.Add(ArtifactSlotType.Weapon, new Artifact(_config.WeaponConfig));
        }

        public bool TryEquipArtifact(IArtifact artifact)
        {
            if (!_artifacts.ContainsKey(artifact.SlotType)) _artifacts.Add(artifact.SlotType, null);
            _artifacts[artifact.SlotType] = artifact;

            return true;

            //Debug.Log($"Artifacts reset. new count {_artifacts.Values.Count}");
        }

        public void RemoveArtifact(IArtifact artifact)
        {
            if (artifact.SlotType != ArtifactSlotType.Weapon
                && _artifacts.ContainsKey(artifact.SlotType)
                && _artifacts[artifact.SlotType] == artifact)
                    _artifacts.Remove(artifact.SlotType);
        }
    }
}