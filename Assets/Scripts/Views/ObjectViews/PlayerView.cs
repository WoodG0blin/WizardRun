using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

namespace WizardsPlatformer
{
    internal interface IPlayerView: IAnimated, IJump, IDamagable
    {
        void SetVelocity(float newVelocityX);
    }

    internal class PlayerView : View, IPlayerView
    {
        [SerializeField] private Transform _barrel;

        private AnimationController _animator;
        public ActionState animationState { get; set; }
        public event Action<float> OnReceiveDamage;

        public void SetVelocity(float newVelocityX)
        {
            if(XDirection * newVelocityX < 0) SetDirection(newVelocityX);

            if (HasNoBarrier(newVelocityX))
                rigidbody.velocity = new Vector2(newVelocityX, rigidbody.velocity.y);
        }
        private bool HasNoBarrier(float direction) => (direction > 0 && !AccessContacts().HasContactRight) || (direction < 0 && !AccessContacts().HasContactLeft);

        public void Jump(float jumpForce)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            _animator.AnimationState(ActionState.Jump, true);
        }

        protected override void OnUpdate()
        {
            //if (Position.y < -15f) ReceiveDamage(1000f);

            if (Mathf.Abs(rigidbody.velocity.x) < 0.1f) _animator.AnimationState(ActionState.Idle);
            else if (Mathf.Abs(rigidbody.velocity.x) < 1f) _animator.AnimationState(ActionState.Walk);
            else _animator.AnimationState(ActionState.Run);

            _animator.Update();
        }

        public void InitiateAnimations(AnimationSequence[] animations)
        {
            RegisterOnUpdate();
            _animator = new AnimationController(
                renderer,
                animations ?? new AnimationSequence[] { new AnimationSequence() { Sprites = new List<Sprite>() { renderer.sprite } } }
                );
        }

        public void ReceiveDamage(float damage) => OnReceiveDamage?.Invoke(damage);

        public Transform GetWeapon() { return _barrel ?? visualBody.Find("Weapon"); }
    }
}
