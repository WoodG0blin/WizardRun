using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(AllLevelObjectsConfigs), menuName = "Configs/" + nameof(AllLevelObjectsConfigs), order = 4)]
    internal class AllLevelObjectsConfigs : ScriptableObject
    {
        [SerializeField] public List<LevelObjectConfig> Elements { get; private set; }
        [SerializeField] public List<LevelObjectConfig> Obstacles { get; private set; }
        [SerializeField] public List<LevelObjectConfig> Enemies { get; private set; }
        [SerializeField] public LevelObjectConfig Bonus { get; private set; }
        [SerializeField] public LevelObjectConfig Portal { get; private set; }
    }
}
