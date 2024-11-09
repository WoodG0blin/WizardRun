using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class CharacterStats
    {
        private int _maxHealth;
        private int _defence;
        private int _speed;
        private int _jumpForce;

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
        
        public CharacterStats(int health = 100, int speed = 3, int jumpforce = 5, Transform parent = null)
        {
            _maxHealth = health;
            _defence = 0;
            _speed = speed;
            _jumpForce = jumpforce;

            _modifiers = new();
        }
        
        public void SetStatsDisplay(IStatsHeadDisplay display)
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