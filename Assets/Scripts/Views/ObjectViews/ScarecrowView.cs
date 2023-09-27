using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class ScarecrowView : LevelObjectView, IKillable
    {
        private IWeapon _weapon;
        private AmmoView _ammo;

        private int _bonusesOnKill;

        private bool _ready;
        private float _targetWaitTime;
        private float _waitTime;
        private float _counter;

        public event System.Action<BonusType, int> OnKilled;
        public event System.Action<float> OnReceiveDamage;

        protected override void OnInit()
        {
            if (config.HasWeapon)
            {
                _targetWaitTime = config.WeaponConfig.CoolDown;
                _waitTime = _targetWaitTime;

                _weapon = Weapon.GetWeapon(transform.Find("Hand"), config.WeaponConfig);
                _weapon.SetDirection(Vector3.up);

                if (_weapon is RangedWeapon rw) _ammo = rw.Ammo;
                if (_ammo != null)
                {
                    _ammo.OnTrigger += () => _ready = true;
                }

                _ready = true;
                _counter = Random.Range(0, _waitTime - 0.1f);
            }

            stats = new Stats(health: config.MaxHealth, parent: transform);
            stats.OnDeath += OnDeath;

            _bonusesOnKill = config.BonusesOnKill;

            RegisterOnUpdate();
        }

        protected override void OnUpdate()
        {
            if (_ready)
            {
                if(_ammo != null && !_ammo.gameObject.activeSelf) _ammo.SetActive(true);

                _counter += Time.deltaTime;

                if (_counter > _waitTime)
                {
                    _ready = false;
                    _weapon.Fire();
                    
                    _counter = 0;
                    _waitTime = Random.Range(_targetWaitTime - 1f, _targetWaitTime + 1f);
                }
            }
        }

        public void ReceiveDamage(float damage) => stats.Health -= damage;

        private void OnDeath()
        {
            OnKilled?.Invoke(BonusType.coin, _bonusesOnKill);
            UnRegisterFromUpdate();
            gameObject.SetActive(false);
        }

        protected override void OnDestruction()
        {
            stats.OnDeath -= OnDeath;
            _ammo.OnTrigger -= () => _ready = true;
            _ammo = null;
        }
    }
}