using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel
    {
        private LevelObjectConfig _config;

        private Dictionary<ArtifactSlotType, IArtifact> _artifacts;

        public string Name { get; private set; }
        public CharacterStats Stats { get; private set; }
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

        public void SetEquippedArtifacts(List<Artifact> artifacts)
        {
            foreach(var a in artifacts)
            {
                if(!_artifacts.ContainsKey(a.SlotType)) _artifacts.Add(a.SlotType, null);
                _artifacts[a.SlotType] = a;
            }
            Debug.Log($"Artifacts reset. new count {_artifacts.Values.Count}");
        }
    }
}