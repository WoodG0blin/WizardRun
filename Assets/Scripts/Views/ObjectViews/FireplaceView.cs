using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class FireplaceView : LevelObjectView, IPlayerPositionObserver, IKillable
    {
        SubscribtableProperty<Vector3> _playerPositionProperty;

        private BallisticAimView _aim;
        private IWeapon _weapon;

        private int _bonusesOnKill;

        public event Action<BonusType, int> OnKilled;
        public event Action<float> OnReceiveDamage;

        protected override void OnInit()
        {
            activeResponse = true;

            if (config.HasWeapon)
            {
                var barrel = transform.Find("Aim");
                if (!barrel.TryGetComponent<BallisticAimView>(out _aim)) _aim = barrel.gameObject.AddComponent<BallisticAimView>();

                _weapon = Weapon.GetWeapon(barrel, config.WeaponConfig);
                if(_weapon is RangedWeapon rw) _aim.Init(config.WeaponConfig.AttackDistance, config.WeaponConfig.FireForce / rw.Ammo.Mass, rw.Ammo.Gravity);    
            }

            stats = new Stats(health: config.MaxHealth, parent: transform);
            stats.OnDeath += OnDeath;

            _bonusesOnKill = config.BonusesOnKill;

            RegisterOnUpdate();
        }

        public void RegisterObserveTarget(SubscribtableProperty<Vector3> observeTarget)
        {
            _playerPositionProperty = observeTarget;
            _playerPositionProperty.SubscribeOnValueChange(OnPlayerPositionChange);
        }

        protected override void OnUpdate()
        {
            if (_weapon.WeaponReady && _aim.InDistance) _weapon.Fire();
        }

        protected void OnPlayerPositionChange(Vector3 newPlayerPosition)
        {
            _aim.UpdateAim(newPlayerPosition);
            _weapon.SetDirection(_aim.Direction);
        }

        protected override void OnDestruction()
        {
            _aim.Dispose();
            _playerPositionProperty.UnsubscribeOnValueChange(OnPlayerPositionChange);
            _playerPositionProperty = null;
            stats.OnDeath -= OnDeath;
        }

        public void ReceiveDamage(float damage) => stats.Health -= damage;

        private void OnDeath()
        {
            OnKilled?.Invoke(BonusType.coin, _bonusesOnKill);
            UnRegisterFromUpdate();
            gameObject.SetActive(false);
        }
    }
}