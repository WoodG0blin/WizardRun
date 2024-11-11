using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal abstract class AmmoView : LevelObjectView
    {
        [SerializeField] protected float lifetime = 5.0f;
        protected bool isFromPlayer = false;

        private TrailRenderer _trailRenderer;

        protected Transform _barrel;

        protected int _damage;
        protected int _speed;
        protected float _baseGravity;
        protected Coroutine _currentTimer;

        public Action OnTrigger;

        //public bool Ready { get; protected set; }
        public float Mass { get => rigidbody.mass; }
        public float Gravity { get => _baseGravity; }

        public void Init(Transform barrel, int damage, int speed)
        {
            SetActive(false);
            _barrel = barrel;
            _damage = damage;
            _speed = speed;
            _baseGravity = rigidbody.gravityScale;
            transform.TryGetComponent<TrailRenderer>(out _trailRenderer);
            //Ready = true;
            SetToBase();
        }

        public void ResetToPlayer()
        {
            isFromPlayer = true;
            OnResetToPlayer();
        }

        protected virtual void OnResetToPlayer() { }

        public void Fire(Vector2 direction)
        {
            //if (Ready)
            //{
                transform.SetParent(null);
                rigidbody.gravityScale = _baseGravity;
                //Ready = false;
                SetActive(true);
                _currentTimer = StartCoroutine(DestroyAfterTime(lifetime));
                OnFire(direction);
            //}
        }
        protected abstract void OnFire(Vector2 direction);

        private void SetToBase()
        {
            //SetActive(false);
            //_trailRenderer?.Clear();

            //if (_currentTimer != null) StopCoroutine(_currentTimer);
            //_currentTimer = null;

            SetPosition(_barrel.position);
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = 0;
            SetRotation(Quaternion.identity);

            //transform.SetParent(_barrel);
            
            //Ready = true;
        }

        protected override void OnCollision(IInteractionResponder interactor)
        {
            if ((isFromPlayer && !interactor.IsPlayer)
                || (!isFromPlayer && interactor.IsPlayer))
                interactor.ReceiveDamage(_damage);
            Destroy();
        }

        private IEnumerator DestroyAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy();
        }

        protected void Destroy()
        {
            if (_currentTimer != null)
            {
                StopCoroutine(_currentTimer);
                _currentTimer = null;
            }
        }
    }
}