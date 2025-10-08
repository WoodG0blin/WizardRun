using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(LevelConfig), menuName = "Configs/" + nameof(LevelConfig), order = 8)]
    public sealed class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite[] BackGroundSprites { get; private set; }
        [field: SerializeField] public GameObject Block { get; private set; }
        [field: SerializeField] public List<LevelObjectConfig> LevelObjects { get; private set; }
        [field: SerializeField] public LevelObjectConfig BasePlatform { get; private set; }
        [field: SerializeField] public LevelObjectConfig Bridge { get; private set; }
        [field: SerializeField, Space(20)] public List<BossGroundConfig> BossGrounds { get; private set; }
        public int LevelLength { get; set; } = 20;
    }
}