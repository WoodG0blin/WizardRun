using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class BonusStats : IEnumerable<KeyValuePair<BonusType, int>>
    {
        private Dictionary<BonusType, int> _bonuses = new Dictionary<BonusType, int>();
        private readonly bool _IsSavable;

        public event Action<BonusType, int> onBonusChange;
        public int this[BonusType T]
        {
            get
            {
                if (!_bonuses.ContainsKey(T)) _bonuses.Add(T, _IsSavable ? PlayerPrefs.GetInt(T.ToString()) : 0);
                return _bonuses[T];
            }
            set
            {
                if (!_bonuses.ContainsKey(T)) _bonuses.Add(T, _IsSavable ? PlayerPrefs.GetInt(T.ToString()) : 0);
                _bonuses[T] = value;
                onBonusChange?.Invoke(T, value);
                if(_IsSavable) PlayerPrefs.SetInt(T.ToString(), _bonuses[T]);
            }
        }

        public BonusStats(bool isSavable = false)
        {
            PlayerPrefs.SetInt(BonusType.coin.ToString(), 0);
            _IsSavable = isSavable;
        }

        public void RefreshValues()
        {
            foreach (KeyValuePair<BonusType, int> bonus in _bonuses)
                onBonusChange?.Invoke(bonus.Key, bonus.Value);
        }

        IEnumerator<KeyValuePair<BonusType, int>> IEnumerable<KeyValuePair<BonusType, int>>.GetEnumerator() =>
            _bonuses.GetEnumerator();

        public IEnumerator GetEnumerator() => _bonuses.GetEnumerator();
    }
}
