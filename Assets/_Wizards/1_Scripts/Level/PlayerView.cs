using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IPlayerView: IJump
    {
        void SetVelocity(float newVelocityX);
        Transform GetBarrelObject();
    }

    internal class PlayerView : LevelObjectView, IPlayerView
    {
        [SerializeField] private Transform _barrel;


        public void SetVelocity(float newVelocityX)
        {
            if(XDirection * newVelocityX < 0) SetXDirection(newVelocityX);

            if (HasNoBarrier(XDirection))
                rigidbody.velocity = new Vector3(newVelocityX, rigidbody.velocity.y, 0);
        }
        private bool HasNoBarrier(float direction) => (direction > 0 && !AccessContacts().HasContactRight) || (direction < 0 && !AccessContacts().HasContactLeft);

        public void Jump(float jumpForce)
        {
            //rigidbody.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.TriggerAnimation(ActionState.Jump);
            //_animator?.AnimationState(ActionState.Jump, true);
        }

        public void DisplayHit() => animator.TriggerAnimation(ActionState.Hurt);

        public void DisplayDying() => animator.TriggerAnimation(ActionState.Die);

        public void DisplayAttack(Action onAttackPositionReady) => animator.TriggerAnimation(ActionState.Attack, onAttackPositionReady);

       
        protected override void OnUpdate()
        {
            animator.UpdateValues(rigidbody.velocity);
        }


        protected override void OnCollision(IInteractionResponder interactor)
        {
            if (!interactor.IsPlayer)
            {
                OnInteraction?.Invoke(interactor);
            }
        }
        public Transform GetBarrelObject() { return _barrel ?? visualBody.Find("Barrel"); }
    }
}
