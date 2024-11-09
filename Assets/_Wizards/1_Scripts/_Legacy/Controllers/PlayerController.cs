using System.Collections.Generic;
using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerController : IUpgradable, IArtifactHolder
    {
        private PlayerModel _playerModel;
        private CharacterStats _stats;
        private UpgradesManager _upgradesManager;

        private float _moveThreshold = 0.02f;
        private float _input = 0f;
        private bool _doWalk = false;

        private Dictionary<ActivatorType, IUpgrade> _upgrades;
        private IArtifactExecutorsContainer _executors;

        private IPlayerView _playerView;
        private IWeapon _weapon;

        public CharacterStats Stats { get => _stats; }
        public Action<Vector3> OnPlayerPositionChange;
        public Action OnPlayerDeath;

        Dictionary<ActivatorType, IUpgrade> IUpgradable.Upgrades => _upgrades;
        CharacterStats IUpgradable.Stats => _stats;
        IJump IUpgradable.Jumper => _playerView;
        IWeapon IUpgradable.Weapon => _weapon;

        bool IArtifactHolder.IsPlayer => true;
        List<IArtifact> IArtifactHolder.EquippedArtifacts => _playerModel.EquippedArtifacts;
        Transform IArtifactHolder.Barrel => _playerView.GetBarrelObject();
        Vector3 IArtifactHolder.Direction => new Vector3(_playerView.XDirection, 0, 0);
        IJump IArtifactHolder.JumpExecutioner => _playerView;

        public PlayerController(PlayerModel playerModel, PlayerView playerView, Vector3 startPosition)
        {
            _playerModel = playerModel;

            _playerView = playerView;
            _playerView.OnReceiveDamage += ReceiveDamage;

            _weapon = _playerModel.GetWeaponTo(playerView.GetBarrelObject());

            _stats = _playerModel.Stats;
            _stats.OnDeath += OnDeath;

            (this as IUpgradable).Reset();
            _upgradesManager = new UpgradesManager(_playerModel.Upgrades);
            _upgradesManager.SetUpgrades(this);

            _executors = _playerModel.Executors;

            _upgrades[ActivatorType.OnStats].Activate();

            SetPosition(startPosition);
        }
            

        public void SetActive(bool active) => _playerView.SetActive(active);
        private void Move()
        {
            _doWalk = Mathf.Abs(_input) > _moveThreshold;
            if (_doWalk) _playerView.SetVelocity(_input * _stats.Speed);

            OnPlayerPositionChange?.Invoke(_playerView.Position);
        }

        public void OnHorizontalMove(float newValue)
        {
            _input = newValue;
            Move();
        }
        public void OnJump()
        {
            if ((_playerView as IJump).AccessContacts().HasContactDown) _playerView.Jump(_stats.JumpForce);

            _executors.ExecuteFor(ArtifactExecutorType.Jump, this);
            //_upgrades[ActivatorType.OnJump].Activate();
        }

        public void OnFire()
        {
            _executors.ExecuteFor(ArtifactExecutorType.Attack, this);
            //_weapon.SetDirection(new Vector3(_playerView.XDirection, 0, 0));
            //if (_weapon.WeaponReady) _weapon.Fire();

            //_upgrades[ActivatorType.OnAttack].Activate();
        }

        public void SetPosition(Vector3 position) => _playerView.SetPosition(position);

        public void ReceiveDamage(int damage)
        {
            _stats.Health -= damage;
        }

        private void OnDeath()
        {
            GameObject.Destroy((_playerView as View).gameObject);
            _stats.OnDeath -= OnDeath;
            (_playerView as IDamagable).OnReceiveDamage -= ReceiveDamage;
            OnPlayerDeath?.Invoke();
        }

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
