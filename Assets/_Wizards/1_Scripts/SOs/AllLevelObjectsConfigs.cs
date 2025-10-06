using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(AllLevelObjectsConfigs), menuName = "Configs/" + nameof(AllLevelObjectsConfigs), order = 4)]
    public class AllLevelObjectsConfigs : ScriptableObject
    {
        [field: SerializeField] public List<LevelObjectConfig> Elements { get; private set; }
        [field: SerializeField] public List<LevelObjectConfig> Obstacles { get; private set; }
        [field: SerializeField] public List<LevelObjectConfig> Enemies { get; private set; }
        [field: SerializeField] public LevelObjectConfig Bonus { get; private set; }
        [field: SerializeField] public LevelObjectConfig Portal { get; private set; }
    }
}
