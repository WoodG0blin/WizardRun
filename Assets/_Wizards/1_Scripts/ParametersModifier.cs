using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardsPlatformer
{
    public class ParametersModifier
    {
        public Dictionary<CharacterStatType, int> BaseModifiers { get; private set; }
        private Dictionary<CharacterStatType, int> _modifiers;

        public int AvailableModsCount { get; private set; } = 0;
        public int UnusedModsCount => AvailableModsCount - GetBaseModsCount();
        public Action OnModified { get; set; }

        private CancellationTokenSource _cancellation;

        public ParametersModifier(List<Modifier> baseModifiers)
        {
            BaseModifiers = new();
            foreach (var m in baseModifiers)
                BaseModifiers.Add(m.type, m.value);
            AvailableModsCount = GetBaseModsCount();
            _modifiers = new();
            _cancellation = new();
        }

        private int GetBaseModsCount()
        {
            int total = 0;
            foreach (var val in BaseModifiers.Values) total += val;
            return total;
        }

        public void AddToAvailableModsCount(int addition) => AvailableModsCount += addition;
        public int GetModifier(CharacterStatType type)
        {
            int res = GetBaseModifier(type);
            if (_modifiers.ContainsKey(type)) res += _modifiers[type];
            return res;
        }

        public int GetBaseModifier(CharacterStatType type)
        {
            if (BaseModifiers.ContainsKey(type)) return BaseModifiers[type];
            return 0;
        }

        public void SetBaseModifier(CharacterStatType type, int value)
        {
            if (!BaseModifiers.ContainsKey(type)) BaseModifiers.Add(type, 0);

            int available = UnusedModsCount + BaseModifiers[type];

            BaseModifiers[type] = Mathf.Min(value, available);

            Debug.Log($"Setting base modifier for {type} at {BaseModifiers[type]} (requested {value})");

            OnModified?.Invoke();
        }

        public void AddModifier(CharacterStatType type, int value)
        {
            if (!_modifiers.ContainsKey(type)) _modifiers.Add(type, 0);
            _modifiers[type] += value;

            Debug.Log($"Adding modifier for {type} at {value})");

            OnModified?.Invoke();
        }

        public async void AddModifierTemp(CharacterStatType type, int value, int seconds)
        {
            AddModifier(type, value);
            try { await Task.Delay(seconds * 1000, _cancellation.Token); }
            catch { }
            AddModifier(type, -value);
        }
        public void CancelAllTempEffects() => _cancellation.Cancel();
    }
}