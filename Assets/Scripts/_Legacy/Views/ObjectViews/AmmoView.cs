using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal abstract class AmmoView : View
    {
        [SerializeField] protected float _lifetime = 5.0f;
        protected bool _isFromPlayer = false;

        protected Transform _barrel;

        protected float _damage;
        protected float _speed;
        protected float _baseGravity;
        protected Coroutine _currentTimer;

        public event Action OnTrigger;

        public bool Ready { get; protected set; }
        public float Mass { get => rigidbody.mass; }
        public float Gravity { get => _baseGravity; }

        public void Init(Transform barrel, float damage, float speed)
        {
            SetActive(false);
            _barrel = barrel;
            _damage = damage;
            _speed = speed;
            _baseGravity = rigidbody.gravityScale;
            Ready = true;
            ReturnToBase();
        }

        public void ResetToPlayer()
        {
            _isFromPlayer = true;
            OnResetToPlayer();
        }

        protected virtual void OnResetToPlayer() { }

        public void Fire(Vector2 direction)
        {
            if (Ready)
            {
                rigidbody.gravityScale = _baseGravity;
                Ready = false;
                SetActive(true);
                _currentTimer = StartCoroutine(DestroyAfterTime(_lifetime));
                OnFire(direction);
            }
        }
        protected abstract void OnFire(Vector2 direction);

        private void ReturnToBase()
        {
            SetActive(false);

            if (_currentTimer != null) StopCoroutine(_currentTimer);
            _currentTimer = null;

            SetPosition(_barrel.position);
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = 0;
            SetRotation(Quaternion.identity);
            
            Ready = true;

            OnReturnToBase();
        }
        protected virtual void OnReturnToBase() { }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<View>(out View view))
                if (view is IDamagable target) OnTriggerExtention(target, collision.tag);
            OnTrigger?.Invoke();
            ReturnToBase();
        }

        protected abstract void OnTriggerExtention(IDamagable target, string tag);

        private IEnumerator DestroyAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            ReturnToBase();
        }

        protected override void OnDestruction()
        {
            if (_currentTimer != null)
            {
                StopCoroutine(_currentTimer);
                _currentTimer = null;
            }
            base.OnDestruction();
        }
    }
}