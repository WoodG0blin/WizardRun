using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WizardsPlatformer
{
    public class LevelObjectFactory : MonoBehaviour
    {
        [SerializeField] private LevelObjectConfig _bonus;
        [SerializeField] private LevelObjectConfig _bossGrounds;

        private List<LevelObjectConfig> _levelObjects;
        private LevelObjectConfig _basePlatform;
        private LevelObjectConfig _bridge;
        private List<BossGroundConfig> _bossConfigs;

        public void InitLevelObjectFactory(LevelConfig configs)
        {
            _levelObjects = configs.LevelObjects;
            _basePlatform = configs.BasePlatform;
            _bridge = configs.Bridge;
            _bossConfigs = configs.BossGrounds;
        }

        internal LevelObject GenerateObjectAt(Vector2Int gridPosition, int difficulty)
        {
            LevelObjectConfig config = GetRandomConfigFromList(_levelObjects, Random.Range(0, difficulty+1));

            if (config == null) return null;

            return GetObjectAt(gridPosition, config);
        }

        internal LevelObject GetObjectAt(Vector2Int gridPosition, LevelObjectConfig config)
        {
            LevelObject res = config.Type switch
            {
                LevelObjectType.Obstacle => new SimpleObject(config, gridPosition),
                LevelObjectType.Trap => new Trap(config, gridPosition),
                LevelObjectType.DirectShooter => new DirectShooter(config, gridPosition),
                LevelObjectType.BallisticShooter => new BallisticShooter(config, gridPosition),
                LevelObjectType.MeleeEnemy => new MeleeEnemy(config, gridPosition),
                _ => new SimpleObject(config, gridPosition)
            };

            return res;
        }

        internal LevelObject GetBridgeAt(Vector2 gridPosition, float angle) => new Bridge(_bridge, gridPosition, angle);
        internal LevelObject GetLiftAt(Vector2 gridPosition, int height) => new Lift(_basePlatform, gridPosition, height);
        internal LevelObject GetJumpPlatformAt(Vector2 gridPosition) => new SimpleObject(_basePlatform, gridPosition);

        //internal LevelObject GetEnemyAt(Vector2Int gridPosition, int difficulty) => new MeleeEnemy(_configs.Enemies[0], gridPosition);

        internal LevelObject GetBonusAt(Vector2Int gridPosition) => new BonusObject(_bonus, gridPosition);

        internal BossGroundConfig GetBossConfig()
        {
            BossGroundConfig res = null;
            if (_bossConfigs.Count > 0)
            {
                res = _bossConfigs[UnityEngine.Random.Range(0, _bossConfigs.Count)];
                res.Grounds = _bossGrounds;
            }
            return res;
        }

        private LevelObjectConfig GetRandomConfigFromList(List<LevelObjectConfig> list, int difficulty)
        {
            var targetList = list.Where(c => c.DifficultyLevel == difficulty).ToList();
            if (targetList != null && targetList.Count > 0)
                return targetList[Random.Range(0, targetList.Count)];
            else return null;
        }
    }
}
