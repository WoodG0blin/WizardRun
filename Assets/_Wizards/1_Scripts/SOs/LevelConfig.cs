using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(LevelConfig), menuName = "Configs/" + nameof(LevelConfig), order = 8)]
    public sealed class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public int LevelLength { get; set; } = 20;
        [field: SerializeField] public Sprite[] BackGroundSprites { get; private set; }
        [field: SerializeField] public GameObject Block { get; private set; }
        [field: SerializeField] public AllLevelObjectsConfigs Objects { get; private set; }
        [field: SerializeField] public List<LevelObjectConfig> BossGrounds { get; private set; }
    }
}