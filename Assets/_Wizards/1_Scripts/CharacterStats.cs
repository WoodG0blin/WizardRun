using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class CharacterStats
    {
        private const float STAT_CHANGE_PER_MODIFIER_POINT = 0.05f;

        private int _maxHealth;
        private int _defence;
        private int _speed;
        private int _damage;

        public ParametersModifier Modifiers { get; private set; }

        private int _currentHealth;
        
        private IStatsHeadDisplay _statsHeadDisplay;
        
        public Action<int> OnCurrentHealthChange { get; set; }
        public Action OnBaseParametersChange { get; set; }
        public Action OnDeath;
        
        public int MaxHealth => Mathf.RoundToInt(_maxHealth * (1f + STAT_CHANGE_PER_MODIFIER_POINT * Modifiers.GetModifier(CharacterStatType.MaxHealth)));
        public float Defence => (_defence + Modifiers.GetModifier(CharacterStatType.Defence)) * STAT_CHANGE_PER_MODIFIER_POINT;
        public float Speed => 1f + (_speed + Modifiers.GetModifier(CharacterStatType.Speed)) * STAT_CHANGE_PER_MODIFIER_POINT;
        public int Damage => Mathf.RoundToInt(_damage * (1f + STAT_CHANGE_PER_MODIFIER_POINT * Modifiers.GetModifier(CharacterStatType.Damage)));
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
        

        public CharacterStats(LevelObjectConfig config, List<Modifier> baseModifiers)
        {
            _maxHealth = config.MaxHealth;
            _currentHealth = _maxHealth;

            _defence = 10;

            _damage = config.Damage;
            _defence = config.Defence;
            _speed = config.Speed;

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