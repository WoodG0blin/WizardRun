using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardsPlatformer
{
    public class ParametersModifier<T> where T: struct, IConvertible
    {
        private Dictionary<T, int> _modifiers;

        private CancellationTokenSource _cancellation;

        public ParametersModifier()
        {
            _modifiers = new();
            _cancellation = new();
        }

        public int GetModifier(T type)
        {
            if (_modifiers.ContainsKey(type)) return _modifiers[type];
            else return 0;
        }
        public void AddModifier(T type, int value)
        {
            if (!_modifiers.ContainsKey(type)) _modifiers.Add(type, 0);
            _modifiers[type] += value;
        }
        public async void AddModifierTemp(T type, int value, int seconds)
        {
            AddModifier(type, value);
            try { await Task.Delay(seconds * 1000, _cancellation.Token); }
            catch { }
            AddModifier(type, -value);
        }
        public void CancelAllTempEffects() => _cancellation.Cancel();
    }
}