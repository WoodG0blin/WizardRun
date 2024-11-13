using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsPlatformer;

namespace WizardsPlatformer
{
    internal class PortalView : LevelObjectView, IAnimatedView
    {
        private AnimationController _animator;
        public ActionState animationState { get; set; }

        public void InitiateAnimations(AnimationSequence[] animations)
        {
            //AnimationSequence[] temp = animations;
            //if (temp == null || temp.Length == 0) temp = new AnimationSequence[] { new AnimationSequence() { Sprites = new List<Sprite>() { renderer.sprite } } };

            //_animator = new AnimationController(
            //    renderer,
            //    temp);
        }

        void Update() => _animator?.Update();

        protected override void OnCollision(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                OnInteraction?.Invoke(interactor);
                SetActive(false);
            }
        }
    }
}