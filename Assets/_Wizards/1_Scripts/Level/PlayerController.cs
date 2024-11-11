using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerController : ActiveObject
    {
        private PlayerView _playerView;
        private PlayerModel _playerModel;

        private Vector3 _startPosition;

        private float _moveThreshold = 0.02f;
        private float _input = 0f;
        private bool _doWalk = false;

        private IArtifactExecutorsContainer _executors;

        public CharacterStats Stats { get => stats; }
        public Action<Vector3> OnPlayerPositionChange;
        public Action OnPlayerDeath;

        public PlayerController(PlayerModel playerModel, Vector3 startPosition) : base(playerModel.Config, new())
        {
            isPlayer = true;

            _playerModel = playerModel;
            //_playerView = playerView;

            EquippedArtifacts = playerModel.EquippedArtifacts;

            stats = _playerModel.Stats;
            stats.Health = stats.MaxHealth;
            stats.OnDeath = Die;

            _executors = _playerModel.Executors;

            _startPosition = startPosition;
        }
            
        private void Move()
        {
            _doWalk = Mathf.Abs(_input) > _moveThreshold;
            if (_doWalk) _playerView.SetVelocity(_input * stats.Speed);

            OnPlayerPositionChange?.Invoke(_playerView.Position);
        }

        public void OnHorizontalMove(float newValue)
        {
            _input = newValue;
            Move();
        }
        public void OnJump()
        {
            if ((_playerView as IJump).AccessContacts().HasContactDown) _playerView.Jump(stats.JumpForce);

            _executors.ExecuteFor(ArtifactExecutorType.Jump, this);
        }

        public void OnFire()
        {
            Direction = new(_playerView.XDirection, 0, 0);
            _executors.ExecuteFor(ArtifactExecutorType.Attack, this);
        }


        protected override void Die()
        {
            view.SetActive(false);
            stats.OnDeath = null;
            OnPlayerDeath?.Invoke();
        }


        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<PlayerView>();

        protected override void OnInitiateView()
        {
            _playerView = view as PlayerView;
            Barrel = _playerView.GetBarrelObject();
            JumpExecutioner = _playerView;
            _playerView.SetPosition(_startPosition);

            base.OnInitiateView();
        }

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            //no explicit actions
        }
    }
}
