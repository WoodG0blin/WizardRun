using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WizardsPlatformer
{
    [Serializable]
    public class Bonus
    {
        public BonusType Type;
        public int Value;

        public Bonus()
        {
            Type = BonusType.Coin;
            Value = 0;
        }

        public Bonus(BonusType type, int value)
        {
            Type = type;
            Value = value;
        }
    }

}
