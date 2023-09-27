using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class LevelObjectsRepository : Repository<string, ILevelObjectConfig, LevelObjectConfig>
    {
        public LevelObjectsRepository(IEnumerable<LevelObjectConfig> configs) : base(configs) { }

        protected override ILevelObjectConfig CreateItem(LevelObjectConfig config) => config;

        protected override string GetKey(LevelObjectConfig config) => config.Name;
    }
}