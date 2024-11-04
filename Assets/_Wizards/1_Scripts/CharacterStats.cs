using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class CharacterStats
    {
        private Stat<CharacterStatType> _maxHealth;
        private Stat<CharacterStatType> _defence;
        private Stat<CharacterStatType> _speed;
        private Stat<CharacterStatType> _jumpForce;

        private int _currentHealth;
        
        private IStatsHeadDisplay _statsHeadDisplay;
        
        public Action<int> OnCurrentHealthChange;
        public Action OnBaseParametersChange;
        public Action OnDeath;
        
        public int MaxHealth => _maxHealth.Value;
        public int Defence => _defence.Value;
        public int Speed => _speed.Value;
        public int JumpForce => _jumpForce.Value;
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
            _maxHealth = new(CharacterStatType.MaxHealth, health);
            _defence = new(CharacterStatType.Defence, 0);
            _speed = new(CharacterStatType.Speed, speed);
            _jumpForce = new(CharacterStatType.JumpForce,jumpforce);
        }
        
        public void SetStatsDisplay(IStatsHeadDisplay display)
        {
            _statsHeadDisplay = display;
            _statsHeadDisplay.Activate();
        }
        
        public void AddStatsModifier(CharacterStatType type, int modifierValue) =>
            GetStatByType(type).AddModifier(modifierValue);
        public void RemoveStatsModifier(CharacterStatType type, int modifierValue) =>
            GetStatByType(type).RemoveModifier(modifierValue);

        private Stat<CharacterStatType> GetStatByType(CharacterStatType type) => type switch
        {
            CharacterStatType.MaxHealth => _maxHealth,
            CharacterStatType.Defence => _defence,
            CharacterStatType.Speed => _speed,
            CharacterStatType.JumpForce => _jumpForce,
            _ => new(CharacterStatType.MaxHealth)
        };
    }
}