using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class CharacterStats
    {
        private int _maxHealth;
        private int _defence;
        private int _speed;
        private int _jumpForce;
        private int _damage;

        public ParametersModifier Modifiers { get; private set; }

        private int _currentHealth;
        
        private IStatsHeadDisplay _statsHeadDisplay;
        
        public Action<int> OnCurrentHealthChange { get; set; }
        public Action OnBaseParametersChange;
        public Action OnDeath;
        
        public int MaxHealth => Mathf.RoundToInt(_maxHealth * (1f + 0.05f * Modifiers.GetModifier(CharacterStatType.MaxHealth)));
        public int Defence => Mathf.RoundToInt(_defence * (1f + 0.05f * Modifiers.GetModifier(CharacterStatType.Defence)));
        public int Speed => Mathf.RoundToInt(_speed * (1f + 0.05f * Modifiers.GetModifier(CharacterStatType.Speed)));
        public int JumpForce => Mathf.RoundToInt(_jumpForce * (1f + 0.05f * Modifiers.GetModifier(CharacterStatType.JumpForce)));
        public int Damage => Mathf.RoundToInt(_damage * (1f + 0.05f * Modifiers.GetModifier(CharacterStatType.Damage)));
        public int Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                OnCurrentHealthChange?.Invoke(value);
                _statsHeadDisplay?.DisplayHealth(_currentHealth / MaxHealth);
                if (_currentHealth == 0) OnDeath?.Invoke();
            }
        }
        

        public CharacterStats(LevelObjectConfig config, Dictionary<CharacterStatType, int> baseModifiers)
        {
            _maxHealth = config.MaxHealth;
            _currentHealth = _maxHealth;

            _defence = 10;

            _damage = config.Damage;

            _speed = config.Speed;
            _jumpForce = config.JumpForce;

            Modifiers = new(baseModifiers);
            Modifiers.OnModified += () => OnBaseParametersChange?.Invoke();
        }
        
        internal void SetStatsDisplay(IStatsHeadDisplay display)
        {
            _statsHeadDisplay = display;
            _statsHeadDisplay.Activate();
        }
        
        public void AddStatsModifier(CharacterStatType type, int modifierValue) =>
            Modifiers.AddModifier(type, modifierValue);
        public void RemoveStatsModifier(CharacterStatType type, int modifierValue) =>
            Modifiers.AddModifier(type, modifierValue);
    }
}