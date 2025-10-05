using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WizardsPlatformer
{
    public class LevelObjectFactory
    {
        private AllLevelObjectsConfigs _configs;
        private List<LevelObjectConfig> _bossGrounds;

        internal LevelObjectFactory(AllLevelObjectsConfigs configs) => _configs = configs;

        internal LevelObject GenerateObstacleAt(Vector2Int gridPosition, int difficulty)
        {
            LevelObjectConfig config = GetRandomConfigFromList(_configs.Obstacles, Random.Range(0, difficulty+1));

            if (config == null) return null;

            LevelObject res = config.Name switch
            {
                "Rock" => new SimpleObject(config, gridPosition),
                "Spikes" => new Trap(config, gridPosition),
                "Bowl" => new DirectShooter(config, gridPosition),
                "Fireplace" => new BallisticShooter(config, gridPosition),
                "Scarecrow" => new Scarecrow(config, gridPosition),
                _ => new SimpleObject (config, gridPosition)
            };

            return res;
        }

        internal LevelObject GetBridgeAt(Vector2 gridPosition, float angle) => new Bridge(_configs.Elements.Where(e => e.Name == "Bridge").First(), gridPosition, angle);
        internal LevelObject GetLiftAt(Vector2 gridPosition, int height) => new Lift(_configs.Elements.Where(e => e.Name == "Lift").First(), gridPosition, height);
        internal LevelObject GetJumpPlatformAt(Vector2 gridPosition) => new SimpleObject(_configs.Elements.Where(e => e.Name == "JumpPlatform").First(), gridPosition);

        internal LevelObject GetEnemyAt(Vector2Int gridPosition, int difficulty) => new MeleeEnemy(_configs.Enemies[0], gridPosition);

        internal LevelObject GetBonusAt(Vector2Int gridPosition) => new Bonus(_configs.Bonus, gridPosition);

        internal LevelObject GetPortalAt(Vector2Int gridPosition) => new Portal(_configs.Portal, gridPosition);

        private LevelObjectConfig GetRandomConfigFromList(List<LevelObjectConfig> list, int difficulty)
        {
            var targetList = list.Where(c => c.DifficultyLevel == difficulty).ToList();
            if (targetList != null && targetList.Count > 0)
                return targetList[Random.Range(0, targetList.Count)];
            else return null;
        }
    }
}
