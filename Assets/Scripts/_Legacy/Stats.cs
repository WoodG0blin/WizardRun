using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Stats
    {
        //REDO config load
        private SubscribrablePropertyWithEqualsCheck<float> _health;
        private float _maxHealth;

        private IStatsHeadDisplay _statsHeadDisplay;

        public event Action OnDeath;

        public Stats(float health = 100, float speed = 3, float jumpforce = 5, Transform parent = null)
        {
            _health = new SubscribrablePropertyWithEqualsCheck<float>();
            _health.Value = health;
            _maxHealth = health;

            Speed = speed;
            JumpForce = jumpforce;
            if(parent != null)_statsHeadDisplay = parent.Find("StatsHeadDisplay").GetComponentInChildren<IStatsHeadDisplay>();
            _statsHeadDisplay?.Activate();
        }

        public float Health
        {
            get => _health.Value;
            set
            {
                _health.Value = Mathf.Clamp(value, 0, _maxHealth);
                _statsHeadDisplay?.DisplayHealth(_health.Value / _maxHealth);
                if (_health.Value == 0) OnDeath?.Invoke();
            }
        }

        public float MaxHealth { get => _maxHealth; }

        public SubscribrablePropertyWithEqualsCheck<float> HealthProperty { get => _health; }

        public float Speed { get; set; }
        public float JumpForce { get; set; }
    }
}