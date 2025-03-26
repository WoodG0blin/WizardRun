using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerController : ActiveObject
    {
        private PlayerView _playerView;
        private PlayerModel _playerModel;

        private int _startHealth;

        private float _moveThreshold = 0.02f;

        private IArtifactExecutorsContainer _executors;

        public Action<Vector3> OnPlayerPositionChange { get; set; }
        public Action OnPlayerDeath;
        public float PlayerDamagedValue => 1f - (float) stats.Health / _startHealth;

        public PlayerController(PlayerModel playerModel, Vector2Int startPosition) : base(playerModel.Config, startPosition)
        {
            isPlayer = true;

            _playerModel = playerModel;

            EquippedArtifacts = playerModel.EquippedArtifacts;

            stats = _playerModel.Stats;
            stats.Health = stats.MaxHealth;
            stats.OnDeath = Die;

            _startHealth = stats.Health;

            _executors = _playerModel.Executors;
        }
            
        public void OnHorizontalMove(float newValue)
        {
            if (Mathf.Abs(newValue) > _moveThreshold)
                _playerView.Mover?.SetMoveTo(newValue, stats.Speed);

            OnPlayerPositionChange?.Invoke(_playerView.Position);
        }
        public void OnJump()
        {
            if (_playerView.Mover.IsGrounded)
                _playerView.Jumper?.Jump(stats.JumpForce);

            _executors.ExecuteFor(ArtifactExecutorType.Jump, this);
        }

        public void OnFire()
        {
            Direction = new(_playerView.XDirection, 0, 0);
            _playerView.DisplayAttack(
                () => _executors.ExecuteFor(ArtifactExecutorType.Attack, this));
        }


        protected override void Die()
        {
            _playerView.DisplayDying();
            view.SetActive(false);
            stats.OnDeath = null;
            OnPlayerDeath?.Invoke();
        }

        public override void Destroy() => ReceiveDamage(stats.Health);

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<PlayerView>();

        protected override void OnInitiateView()
        {
            _playerView = view as PlayerView;
            Barrel = _playerView.GetBarrelObject();

            OnReceiveDamage += (d) => _playerView.DisplayHit();

            base.OnInitiateView();
        }

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            //no explicit actions
        }
    }
}
