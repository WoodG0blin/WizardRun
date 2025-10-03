using System;
using System.Collections;
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

        private ParametersModifier<CharacterStatType> _modifiers;

        private int _currentHealth;
        
        private IStatsHeadDisplay _statsHeadDisplay;
        
        public Action<int> OnCurrentHealthChange;
        public Action OnBaseParametersChange;
        public Action OnDeath;
        
        public int MaxHealth => _maxHealth + _modifiers.GetModifier(CharacterStatType.MaxHealth);
        public int Defence => _defence + _modifiers.GetModifier(CharacterStatType.Defence);
        public int Speed => _speed + _modifiers.GetModifier(CharacterStatType.Speed);
        public int JumpForce => _jumpForce + _modifiers.GetModifier(CharacterStatType.JumpForce);
        public int Damage => _damage + _modifiers.GetModifier(CharacterStatType.Damage);
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
        

        public CharacterStats(LevelObjectConfig config)
        {
            _maxHealth = config.MaxHealth;
            _currentHealth = _maxHealth;

            _defence = 0;

            _damage = config.Damage;

            _speed = config.Speed;
            _jumpForce = config.JumpForce;

            _modifiers = new();
        }
        
        internal void SetStatsDisplay(IStatsHeadDisplay display)
        {
            _statsHeadDisplay = display;
            _statsHeadDisplay.Activate();
        }
        
        public void AddStatsModifier(CharacterStatType type, int modifierValue) =>
            _modifiers.AddModifier(type, modifierValue);
        public void RemoveStatsModifier(CharacterStatType type, int modifierValue) =>
            _modifiers.AddModifier(type, modifierValue);
        public void ClearAllModifiers()
        {
            _modifiers.CancelAllTempEffects();
            _modifiers = new();
        }
    }
}