using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsPlatformer;

namespace WizardsPlatformer
{
    internal class PortalView : LevelObjectView, IAnimatedView, IPortal
    {
        private bool _triggered = false;

        private AnimationController _animator;
        public ActionState animationState { get; set; }

        public Action onPortalEnter { get; set; }


        public void InitiateAnimations(AnimationSequence[] animations)
        {
            _animator = new AnimationController(
                renderer,
                animations ?? new AnimationSequence[] { new AnimationSequence() { Sprites = new List<Sprite>() { renderer.sprite } } }
                );
        }


        void Update() => _animator.Update();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_triggered && collision.attachedRigidbody.CompareTag("Player"))
            {
                _triggered = true;
                onPortalEnter?.Invoke();
            }
        }
        protected override void OnDestruction()
        {
            onPortalEnter = null;
        }
    }
}