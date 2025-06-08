using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerController : ActiveObject
    {
        private PlayerView _playerView;
        private PlayerModel _playerModel;

        private int _startHealth;

        private float _moveThreshold = 0.02f;

        //private IArtifactExecutorsContainer _executors;
        private List<IArtifact> _artifacts;

        private Action<Vector3> OnPlayerPositionChange;


        public Action OnPlayerDeath;
        public float PlayerHealthValue => (float) stats.Health / _startHealth;


        public PlayerController(PlayerModel playerModel, Vector2Int startPosition) : base(playerModel.Config, startPosition)
        {
            IsPlayer = true;

            _playerModel = playerModel;


            stats = _playerModel.Stats;
            stats.Health = stats.MaxHealth;
            stats.OnDeath = Die;

            _startHealth = stats.Health;

            //_executors = _playerModel.Executors;

            _artifacts = new();
            foreach(var art in _playerModel.EquippedArtifacts)
            {
                art.SetHolder(this);
                _artifacts.Add(art);
            }

            EquippedArtifacts = _artifacts;

            weapon = _playerModel.Weapon;
            weapon.SetHolder(this);
        }

        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            OnPlayerPositionChange += (v) => subscriber.OnPlayerPositionChange?.Invoke(v);
        }

        public void OnHorizontalMove(float newValue)
        {
            if (Mathf.Abs(newValue) > _moveThreshold)
                _playerView.Mover?.SetInput(new(newValue, 0), stats.Speed);

            OnPlayerPositionChange?.Invoke(_playerView.Position);
        }
        public void OnJump()
        {
            if (_playerView.Mover.IsGrounded)
                _playerView.Jumper?.Jump(stats.JumpForce);

            //_executors.ExecuteFor(ArtifactExecutorType.Jump, this);
            foreach (var art in _artifacts) art.TryUseFor(Artifact.ArtifactActivatorTypes.Jump);
        }

        public void OnFire()
        {
            Direction = new(_playerView.XDirection, 0);
            
            if (weapon.IsReady)
                _playerView.DisplayAttack(onAttackPositionReady: FireAttack);
        }

        private void FireAttack()
        {
            weapon.Fire(Direction);
            foreach (var art in _artifacts) art.TryUseFor(Artifact.ArtifactActivatorTypes.Attack);
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
            barrel = _playerView.GetBarrelObject();

            OnReceiveDamage += (d) => _playerView.DisplayHit();

            base.OnInitiateView();
        }

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            //no explicit actions
        }
    }
}
