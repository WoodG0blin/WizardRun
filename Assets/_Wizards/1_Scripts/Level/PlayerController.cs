using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerController : ActiveObject
    {
        private PlayerView _playerView;
        private PlayerModel _playerModel;

        private int _startHealth;

        private float _moveThreshold = 0.02f;

        private Action<Vector3> OnPlayerPositionChange;

        public Action OnPlayerDeath;
        public float PlayerHealthValue => (float) Stats.Health / _startHealth;


        public PlayerController(PlayerModel playerModel, Vector2Int startPosition) : base(playerModel.Config, startPosition)
        {
            IsPlayer = true;
            

            _playerModel = playerModel;

            Stats = _playerModel.Stats;
            Stats.Health = Stats.MaxHealth;
            Stats.OnDeath = Die;

            Actions = _playerModel.Actions;

            _startHealth = Stats.Health;

            SetUpExplicitExecutors();
        }

        private void SetUpExplicitExecutors()
        {
            var explicits = Actions.GetActionsFor(PropertyActivators.Explicit);

            for(int i = 0; i < explicits.Count; i++)
            {
                if (i == 0) Weapon = explicits[0];
                //else set for buttons
            }
        }

        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            OnPlayerPositionChange += (v) => subscriber.OnPlayerPositionChange?.Invoke(v);
        }

        public void OnHorizontalMove(float newValue)
        {
            if (Mathf.Abs(newValue) > _moveThreshold)
                _playerView.Mover?.SetInput(new(newValue, 0), Stats.Speed);

            OnPlayerPositionChange?.Invoke(_playerView.Position);
        }
        public void OnJump()
        {
            if (_playerView.Mover.IsGrounded)
                _playerView.Mover?.Jump(Stats.JumpForce);

            foreach (var ex in Actions.GetActionsFor(PropertyActivators.OnJump))
                ex.Use(this);

            //_actions.TryUseForAction(Artifact.ArtifactActivatorTypes.Jump);
        }

        public void OnFire()
        {
            Direction = new(_playerView.XDirection, 0);
            
            if (Weapon.IsReady)
                _playerView.DisplayAttack(onAttackPositionReady: Attack);
        }

        private void Attack()
        {
            Weapon.Use(this);

            foreach (var ex in Actions.GetActionsFor(PropertyActivators.OnAttack))
                ex.Use(this);
            //_actions.TryUseForAction(Artifact.ArtifactActivatorTypes.Attack);
        }

        protected override void Die()
        {
            _playerView.DisplayDying();
            view.SetActive(false);
            Stats.OnDeath = null;
            OnPlayerDeath?.Invoke();
        }

        public override void Destroy() => ReceiveDamage(Stats.Health);

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
