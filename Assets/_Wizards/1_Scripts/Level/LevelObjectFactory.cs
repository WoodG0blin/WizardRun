using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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

        private int _currentDifficulty;
        private int _currentLength;

        public void InitLevelObjectFactory(LevelConfig configs, int difficulty, int length)
        {
            _currentDifficulty = difficulty;
            _currentLength = length;

            _levelObjects = configs.LevelObjects;
            _basePlatform = configs.BasePlatform;
            _bridge = configs.Bridge;
            _bossConfigs = configs.BossGrounds;
        }

        internal List<LevelObject> GenerateObjectsFor(Platform platform, int xOffset)
        {
            List<LevelObject> res = new();

            _currentLength -= platform.Length;
            int density = _currentDifficulty > 5 ? 2 : 3;
            int count = platform.Length / density;

            for (int i = 0; i < count; i++)
            {
                Vector2Int gridPosition = platform.GetFreeSpace + new Vector2Int(xOffset, 0);

                int maxAvailable = getAvailableDifficultyIndex();
                var targetList = _levelObjects.Where(c => c.DifficultyLevel == maxAvailable).ToList();

                LevelObjectConfig config = (targetList != null && targetList.Count > 0) ?
                    targetList[Random.Range(0, targetList.Count)] : null;

                if (config != null) res.Add(GetObjectAt(gridPosition, config));
            }
            return res;
        }

        private int getAvailableDifficultyIndex()
        {
            //redo to difficulty tables

            int difficulty = Random.Range(0, _currentDifficulty);

            int maxAvailable = 0;
            foreach (var lo in _levelObjects)
                if (difficulty >= lo.DifficultyLevel && lo.DifficultyLevel > maxAvailable)
                    maxAvailable = lo.DifficultyLevel;

            //add corrections for max levels according to length

            return maxAvailable;
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
