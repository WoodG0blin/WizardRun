using System;
using UnityEngine;

namespace WizardsPlatformer
{
    public class Stat<T> where T: struct, IConvertible
    {
        public T StatType { get; private set; }

        private int _baseValue;
        private int _modifierValue;

        public int Value => _baseValue + _modifierValue;

        public Stat(T statType, int value)
        {
            StatType = statType;
            SetNewBaseValue(value);
            ClearModifier();
        }
        public Stat(T statType) : this(statType, 0) { }

        public void AddModifier(int modifierValue) => _modifierValue += modifierValue;
        public void RemoveModifier(int modifierValue) => _modifierValue -= modifierValue;
        public void ClearModifier() => _modifierValue = 0;

        public void SetNewBaseValue(int value) => _baseValue = Mathf.Max(0, value);
    }
}