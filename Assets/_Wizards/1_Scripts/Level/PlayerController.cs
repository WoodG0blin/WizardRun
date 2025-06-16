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

        private Dictionary<Artifact.ArtifactActivatorTypes, ArtifactActor> _artifactActors;
        private List<ArtifactActor> _explicitActors;

        private Dictionary<Artifact.ArtifactActivatorTypes, List<ArtifactActor>> _actors;

        private ActionsHolder _actions;

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

            SetUpActors(_playerModel.ArtifactActors);
        }

        private void SetUpActors(List<ArtifactActor> actors)
        {
            _actions = _playerModel.Actions;
            _actions.SetHolder(this);
            weapon = _actions.Weapon;

            ////legacy
            //_artifactActors = new();
            //_explicitActors = new();

            //foreach(var actor in actors)
            //{
            //    actor.SetHolder(this);
            //    if(actor.ActivatorType == Artifact.ArtifactActivatorTypes.ExplicitAction) _explicitActors.Add(actor);
            //    else _artifactActors.Add(actor.ActivatorType, actor);
            //}
            ////end legacy

            //_actors = _playerModel.Actors;
            //foreach (var list in _actors.Values)
            //    foreach (var actor in list)
            //        actor.SetHolder(this);

            //if (!_actors.ContainsKey(Artifact.ArtifactActivatorTypes.Attack)) _actors.Add(Artifact.ArtifactActivatorTypes.Attack, new() { weapon as ArtifactActor });
            //else if (!_actors[Artifact.ArtifactActivatorTypes.Attack][0].IsMain) _actors[Artifact.ArtifactActivatorTypes.Attack].Insert(0, weapon as ArtifactActor);

            ////legacy
            //if (_artifactActors.ContainsKey(Artifact.ArtifactActivatorTypes.Attack))
            //{
            //    if (_artifactActors[Artifact.ArtifactActivatorTypes.Attack] is IWeapon externalWeapon) weapon = externalWeapon;
            //    else (weapon as AttackActor).AddInternalActor(_artifactActors[Artifact.ArtifactActivatorTypes.Attack]);
            //}
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

            _actions.TryUseForAction(Artifact.ArtifactActivatorTypes.Jump);

            //_executors.ExecuteFor(ArtifactExecutorType.Jump, this);
            //foreach (var art in _artifacts) art.TryUseFor(Artifact.ArtifactActivatorTypes.Jump);
            //if (_artifactActors.ContainsKey(Artifact.ArtifactActivatorTypes.Jump))
            //    _artifactActors[Artifact.ArtifactActivatorTypes.Jump].Use();
        }

        public void OnFire()
        {
            Direction = new(_playerView.XDirection, 0);
            
            if (weapon.IsReady)
                _playerView.DisplayAttack(onAttackPositionReady: Attack);
        }

        private void Attack()
        {
            weapon.Use();
            _actions.TryUseForAction(Artifact.ArtifactActivatorTypes.Attack);
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
