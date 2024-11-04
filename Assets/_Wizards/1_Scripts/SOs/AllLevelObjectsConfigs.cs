using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(AllLevelObjectsConfigs), menuName = "Configs/" + nameof(AllLevelObjectsConfigs), order = 4)]
    internal class AllLevelObjectsConfigs : ScriptableObject, IDataSource<LevelObjectConfig>
    {
        [SerializeField] private LevelObjectConfig[] _configs;
        public IReadOnlyList<LevelObjectConfig> Configs => _configs;
    }
}
