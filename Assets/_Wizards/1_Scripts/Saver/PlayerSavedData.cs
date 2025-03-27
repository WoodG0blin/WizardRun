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
        public int Bonuses;

        public List<Location> Locations = new();
    }

    [Serializable]
    public class Location
    {
        public LocationType Type;
        public string SpriteID;

        public int Score;

        [NonSerialized] public Sprite Sprite;
        [NonSerialized] private int _extraScore;

        public void AccountForScore(int score)
        {
            int prev = Score;
            Score = Math.Clamp(Score + score, 0, 100);
            _extraScore = Score - prev;
        }

        public (int baseScore, int extraScore) GetDisplayValues()
        {
            var res = (Score - _extraScore, _extraScore);
            _extraScore = 0;
            return res;
        }
    }
}
