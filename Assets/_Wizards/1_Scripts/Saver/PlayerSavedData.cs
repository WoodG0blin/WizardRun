using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [Serializable]
    public class PlayerSavedData
    {
        public string Name;
        public int LastEntryDate;

        public List<Bonus> BonusStats = new();

        public int MasteryLevel;

        public List<ItemConfig> EquipedArtifacts = new();
        public List<ItemConfig> ChestArtifacts = new();

        //public Dictionary<CharacterStatType, int> BaseStatsModifiers = new();
        public List<Modifier> BaseStatsModifiers = new();

        public List<Location> Locations = new();
    }

    [Serializable]
    public class Location
    {
        public LocationType Type;
        public string SpriteID;

        public int Score;

        [NonSerialized] public Sprite Sprite;
        [NonSerialized] public LevelConfig LevelConfig;
        [NonSerialized] private int _baseScore;


        public void SetConfig(LocationConfig config)
        {
            Type = config.Type;
            SpriteID ??= config.SetSprite();
            Sprite = config.GetSprite(SpriteID);
            LevelConfig = config.LevelConfig;
        }

        public void AccountForScore(int score)
        {
            //int prev = Score;
            Score = Math.Clamp(Score + score, 0, 100);
            //_baseScore = Score - prev;
        }

        public (int baseScore, int extraScore) GetDisplayValues()
        {
            var res = (_baseScore, Score - _baseScore);
            SetBaseScore();
            return res;
        }

        public void SetBaseScore() => _baseScore = Score;

        public LevelConfig GetLevelConfig(int length)
        {
            LevelConfig.LevelLength = length;
            return LevelConfig;
        }
    }

    [Serializable]
    public class Modifier
    {
        public CharacterStatType type;
        public int value;

        public Modifier() { }
        public Modifier(CharacterStatType type, int value)
        {
            this.type = type;
            this.value = value;
        }
    }

}
