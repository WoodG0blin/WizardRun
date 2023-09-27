using JoostenProductions;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerController : Controller, IUpgradable
    {
        private readonly LevelModel _levelModel;

        private PlayerModel _playerModel;
        private Stats _stats;
        private UpgradesManager _upgradesManager;

        private float _moveThreshold = 0.02f;
        private float _input = 0f;
        private bool _doWalk = false;

        private Dictionary<ActivatorType, IUpgrade> _upgrades;

        private PlayerView _playerView;
        private IWeapon _weapon;

        public IPlayerView PlayerView { get => _playerView; }
        public Stats Stats { get => _stats; }

        Dictionary<ActivatorType, IUpgrade> IUpgradable.Upgrades => _upgrades;
        Stats IUpgradable.Stats => _stats;
        IJump IUpgradable.Jumper => PlayerView;
        IWeapon IUpgradable.Weapon => _weapon;

        public PlayerController(PlayerModel playerModel, PlayerView playerView, ILevelObjectConfig config)
        {
            _playerModel = playerModel;

            _levelModel = _playerModel.LevelModel;
            _levelModel.HorizontalMove.SubscribeOnValueChange(OnHorizontalMove);
            _levelModel.Jump.SubscribeOnValueChange(OnJump);
            _levelModel.Fire.SubscribeOnValueChange(OnFire);

            _playerView = playerView;
            (_playerView as IDamagable).OnReceiveDamage += ReceiveDamage;

            if (config.HasWeapon)
            {
                _weapon = Weapon.GetWeapon(playerView.GetWeapon(), config.WeaponConfig);
            }

            _stats = new Stats(health: config.MaxHealth, parent: playerView.transform);
            _stats.OnDeath += OnDeath;

            (this as IUpgradable).Reset();
            _upgradesManager = new UpgradesManager(_playerModel.Upgrades);
            _upgradesManager.SetUpgrades(this);

            _upgrades[ActivatorType.OnStats].Activate();
        }

        public PlayerController(PlayerModel playerModel, PlayerView playerView, ILevelObjectConfig config, Vector3 startPosition) : this(playerModel, playerView, config) =>
            SetPosition(startPosition);

        public void SetActive(bool active) => _playerView.SetActive(active);
        private void Move()
        {
            _doWalk = Mathf.Abs(_input) > _moveThreshold;
            if (_doWalk) _playerView.SetVelocity(_input * _stats.Speed);

            _levelModel.PlayerPosition.Value = _playerView.Position;
        }

        public void OnHorizontalMove(float newValue)
        {
            _input = newValue;
            Move();
        }
        public void OnJump(bool jump)
        {
            if (PlayerView.AccessContacts().HasContactDown) PlayerView.Jump(_stats.JumpForce);

            _upgrades[ActivatorType.OnJump].Activate();
        }

        public void OnFire(bool fire)
        {
            _weapon.SetDirection(new Vector3(_playerView.XDirection, 0, 0));
            if(_weapon.WeaponReady) _weapon.Fire();

            _upgrades[ActivatorType.OnAttack].Activate();
        }

        public void SetPosition(Vector3 position) { _playerView.transform.position = position; }

        public void ReceiveDamage(float damage)
        {
            _stats.Health -= damage;
        }
        protected override void OnDispose()
        {
            UpdateManager.UnsubscribeFromUpdate(Move);
            _levelModel.HorizontalMove.UnsubscribeOnValueChange(OnHorizontalMove);
            _levelModel.Jump.UnsubscribeOnValueChange(OnJump);
            _levelModel.Fire.UnsubscribeOnValueChange(OnFire);
            GameObject.Destroy((PlayerView as View).gameObject);
            _stats.OnDeath -= OnDeath;
            (_playerView as IDamagable).OnReceiveDamage -= ReceiveDamage;
        }

        private void OnDeath() => _levelModel.LevelState.Value = LevelState.Finished;

        void IUpgradable.Reset()
        {
            _upgrades = new Dictionary<ActivatorType, IUpgrade>
            {
                [ActivatorType.OnStats] = new StubUpgrade(),
                [ActivatorType.OnJump] = new StubUpgrade(),
                [ActivatorType.OnAttack] = new StubUpgrade()
            };
        }
    }
}
