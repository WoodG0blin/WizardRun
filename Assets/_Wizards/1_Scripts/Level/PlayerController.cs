using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerController : ActiveObject
    {
        private PlayerView _playerView;
        private IPlayerModel _playerModel;

        private int _startHealth;

        private float _moveThreshold = 0.02f;
        private float _jumpThreshold = 0.2f;
        private float _lastVerticalInput = 0f;

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
        public void SubscribeOnInput(IInputView input)
        {
            input.OnMoveInput += SetMoveInput;
            input.OnFireInput += SetFire;

            OnPlayerDeath += () =>
            {
                input.OnMoveInput -= SetMoveInput;
                input.OnFireInput -= SetFire;
            };
        }
        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            OnPlayerPositionChange += (v) => subscriber.OnPlayerPositionChange?.Invoke(v);
            subscriber.OnExitAvailable += SetExit;
            OnPortalExit += subscriber.SetLevelCleared;
        }

        private void SetMoveInput(Vector2 input)
        {
            float xInput = input.x;
            float yInput = input.y;
            bool jump = yInput > _lastVerticalInput;

            if (Mathf.Abs(xInput) > _moveThreshold) SetMove(xInput);
            if (Mathf.Abs(yInput) > _jumpThreshold && jump) SetJump();

            _lastVerticalInput = yInput;
        }

        private void SetMove(float newValue)
        {
            if (Mathf.Abs(newValue) > _moveThreshold)
                _playerView.Mover?.SetInput(new(newValue, 0), Stats.Speed);
        }
        private void SetJump()
        {
            if (_playerView.Mover.IsGrounded)
                _playerView.Mover?.Jump(Stats.Speed);

            foreach (var ex in Actions.GetActionsFor(PropertyActivators.OnJump))
                ex.Use(this);
        }

        private void SetFire()
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
