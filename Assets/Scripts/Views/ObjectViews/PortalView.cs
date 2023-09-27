using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsPlatformer;

namespace WizardsPlatformer
{
    internal class PortalView : View, ILevelObjectView, IInteractive, IAnimated, ILevelStateObserver
    {
        private AnimationController _animator;
        private SubscribtableProperty<GameState> _gameState;
        public ActionState animationState { get; set; }

        public event Action<IInteractive> onTrigger;
        public event Action onPortalEnter;

        public void Draw(Vector3 position)
        {
            SetPosition(position);
            SetActive(true);
        }

        public void Init(Action<IInteractive> onPortalEnter) => onTrigger += onPortalEnter;

        public void InitiateAnimations(AnimationSequence[] animations)
        {
            RegisterOnUpdate();
            _animator = new AnimationController(
                renderer,
                animations ?? new AnimationSequence[] { new AnimationSequence() { Sprites = new List<Sprite>() { renderer.sprite } } }
                );
        }

        public void Interact(Controller target) { }

        public void RegisterLevelState(SubscribtableProperty<GameState> gameState) =>
            _gameState = gameState;

        protected override void OnUpdate() => _animator.Update();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody.CompareTag("Player"))
            {
                onTrigger?.Invoke(this);
                onPortalEnter?.Invoke();
            }
        }
        protected override void OnDestruction()
        {
            onPortalEnter = null;
        }
    }
}