using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal enum BonusType { coin }

    internal class Bonus
    {
        public BonusType Type { get; private set; }
        public int Value { get; private set; }
        public Bonus(BonusType type, int value)
        {
            Type = type;
            Value = value;
        }

    }
}
