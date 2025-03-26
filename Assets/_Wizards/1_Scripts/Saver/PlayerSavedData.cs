using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [Serializable]
    public struct PlayerSavedData
    {
        public string Name;
        public int LastEntryDate;
        public int Score;
        public int Bonuses;
    }

    [Serializable]
    public struct Location
    {
        public int Type;
        public float Score;
    }
}
