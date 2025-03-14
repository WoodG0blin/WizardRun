using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private bool _isAnimated;

       
        internal void Init()
        {
            _animator = transform.GetComponent<Animator>();
            _isAnimated = _animator != null;
        }

        private Action _onAttackPositionReady;

        internal void UpdateValues(Vector3 velocity)
        {
            if (_isAnimated)
            {
                _animator.SetBool("IsMoving", Mathf.Abs(velocity.x) > 0.1f);
                _animator.SetBool("IsJumping", Mathf.Abs(velocity.y) > 0.1f);
            }
        }

        internal void TriggerAnimation(ActionState newState, Action onTrigger = null)
        {
            if (_isAnimated)
                switch(newState)
                {
                    case ActionState.Attack:
                        {
                            _onAttackPositionReady = onTrigger;
                            _animator.SetTrigger("Attack");
                            break;
                        }
                    case ActionState.Jump: _animator.SetTrigger("Jump"); break;
                    case ActionState.Hurt: _animator.SetTrigger("Hit"); break;
                    case ActionState.Die: _animator.SetBool("IsDead", true); break;
                }
            else onTrigger?.Invoke();
        }

        // external method for animator controller (attack animation event)
        public void SetAttackPositionReady() => _onAttackPositionReady?.Invoke();
    }
}
