using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [Serializable]
    public class BossGroundConfig
    {
        [field: SerializeField] public int MainPlatformLength { get; private set; } = 15;
        [field: SerializeField] public List<PlatformConfig> ExtraPlatforms { get; private set; }
        [field: SerializeField] public LevelObjectConfig Boss { get; private set; }
    }

    [Serializable]
    public class PlatformConfig
    {
        public int StartGap;
        public int Length;
        public int Height;
    }

}