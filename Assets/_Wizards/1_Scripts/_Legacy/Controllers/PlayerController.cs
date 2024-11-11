using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerController : ActiveObject
    {
        private PlayerView _playerView;
        private PlayerModel _playerModel;
        private CharacterStats _stats;

        private float _moveThreshold = 0.02f;
        private float _input = 0f;
        private bool _doWalk = false;

        private IArtifactExecutorsContainer _executors;

        public CharacterStats Stats { get => _stats; }
        public Action<Vector3> OnPlayerPositionChange;
        public Action OnPlayerDeath;

        public PlayerController(PlayerModel playerModel, Vector3 startPosition) : base(playerModel.Config, new())
        {
            isPlayer = true;

            _playerModel = playerModel;
            //_playerView = playerView;

            Barrel = _playerView.GetBarrelObject();
            EquippedArtifacts = playerModel.EquippedArtifacts;
            JumpExecutioner = _playerView;

            _stats = _playerModel.Stats;
            _stats.OnDeath = Die;

            _executors = _playerModel.Executors;

            _playerView.SetPosition(startPosition);
        }
            
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
        }

        public void OnFire()
        {
            _executors.ExecuteFor(ArtifactExecutorType.Attack, this);
        }


        protected override void Die()
        {
            view.SetActive(false);
            _stats.OnDeath = null;
            OnPlayerDeath?.Invoke();
        }


        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<PlayerView>();

        protected override void OnInitiateView() => _playerView = view as PlayerView;

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            //no explicit actions
        }
    }
}
