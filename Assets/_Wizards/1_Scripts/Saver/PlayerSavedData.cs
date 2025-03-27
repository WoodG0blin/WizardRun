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
        public int Score;
        public int Bonuses;

        public List<Location> Locations = new();
    }

    [Serializable]
    public class Location
    {
        public LocationType Type;
        public string SpriteID;

        [NonSerialized] public Sprite Sprite;
    }
}
