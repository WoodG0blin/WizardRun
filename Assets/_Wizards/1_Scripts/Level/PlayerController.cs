using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerController : ActiveObject
    {
        private PlayerView _playerView;
        private IPlayerModel _playerModel;

        private int _startHealth;

        private float _jumpThreshold = 0.5f;
        private float _lastVerticalInput = 0f;

        private List<IArtifactExecutor> _explicits = new();

        private Action<Vector3> OnPlayerPositionChange;
        private Action OnPortalExit;

        public Action OnPlayerDeath { get; set; }
        public float PlayerHealthValue => (float) Stats.Health / _startHealth;


        public PlayerController(IPlayerModel playerModel, Vector2Int startPosition) : base(playerModel.Config, startPosition)
        {
            IsPlayer = true;
            

            _playerModel = playerModel;

            Stats = _playerModel.Stats;
            Stats.Health = Stats.MaxHealth;
            Stats.OnDeath = Die;

            Actions = _playerModel.Actions;

            _startHealth = Stats.Health;

            _explicits = Actions.GetActionsFor(PropertyActivators.Explicit);
            Weapon = _explicits.Where(e => e.ControlIndex == 0).FirstOrDefault();
        }

        public void SubscribeOnInput(IInputView input)
        {
            input.OnMoveInput += SetMoveInput;
            input.OnJump += SetJump;

            input.SetExplicitActions(_explicits, UseExplicit);

            OnPlayerDeath += input.ClearInputs;
        }
        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            OnPlayerPositionChange += (v) => subscriber.OnPlayerPositionChange?.Invoke(v);
            subscriber.OnExitAvailable += SetExit;
            OnPortalExit += subscriber.SetLevelCleared;
        }

        private void SetMoveInput(Vector2 input)
        {
            //bool jump = input.y > _lastVerticalInput;

            _playerView.Mover?.SetInput(input, Stats.Speed);
            //if (Mathf.Abs(input.y) > _jumpThreshold && jump) SetJump();

            //_lastVerticalInput = input.y;
        }

        private void SetJump()
        {
            if (_playerView.Mover.IsGrounded)
                _playerView.Mover?.Jump(Stats.Speed);

            foreach (var ex in Actions.GetActionsFor(PropertyActivators.OnJump))
                ex.Use(this);
        }

        private void UseExplicit(IArtifactExecutor choice)
        {
            Direction = new(_playerView.XDirection, 0);

            if(choice.IsReady)
            {
                if(choice == Weapon) _playerView.DisplayAttack(onAttackPositionReady: Attack);
                else choice.Use(this);
            }
        }

        private void Attack()
        {
            Weapon.Use(this);

            foreach (var ex in Actions.GetActionsFor(PropertyActivators.OnAttack))
                ex.Use(this);
        }

        private void SetExit()
        {
            _playerView.InitiatePortal(OnPortalExit, new(3,0,0));
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

            _playerView.SetUpdateActions(() => OnPlayerPositionChange?.Invoke(_playerView.Position));

            base.OnInitiateView();

            _playerView.InitiatePortal(null, Vector3.zero);
        }

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            //no explicit actions
        }
    }
}
