using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel
    {
        private LevelObjectConfig _config;
        private List<UpgradeConfig> _upgrades;
        private WeaponConfig _weaponConfig;

        private Dictionary<ArtifactSlotType, IArtifact> _artifacts;

        public string Name { get; private set; }
        public CharacterStats Stats { get; private set; }

        public IArtifactExecutorsContainer Executors => new ArtifactExecutorsContainer(_artifacts.Values.ToList());
        public IArtifact Weapon { get; private set; }

        public List<IArtifact> EquippedArtifacts => _artifacts.Values.ToList();
        public IReadOnlyList<UpgradeConfig> Upgrades => _upgrades;
        public GameObject Prefab => _config.Prefab;
        public AnimationSequence[] Animations => _config.Animations;
        public IWeapon GetWeaponTo(Transform barrel) => WeaponLegacy.GetWeapon(barrel, _weaponConfig);


        public PlayerModel()
        {
            _upgrades = new List<UpgradeConfig>();
            _artifacts = new();
        }

        public void SetConfig(LevelObjectConfig config)
        {
            _config = config;
            _weaponConfig = config.WeaponConfigLegacy;

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

        public List<IArtifactExecutor> GetExecutors(ArtifactExecutorType type)
        {
            List<IArtifactExecutor> res = new();

            foreach(var a in _artifacts.Values.ToList())
            {
                var ex = a.GetExecutor(type);
                if (ex != null) res.Add(ex);
            }

            return res;
        }

        public void Reset() => _upgrades.Clear();
    }
}