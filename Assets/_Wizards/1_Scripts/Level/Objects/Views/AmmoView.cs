using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal abstract class AmmoView : LevelObjectView
    {
        [SerializeField] protected float lifetime = 5.0f;
        protected bool isFromPlayer = false;

        protected new Rigidbody rigidbody;
        private TrailRenderer _trailRenderer;

        protected Transform _barrel;

        protected int _damage;
        protected float _speed;
        protected float _baseGravity;
        protected bool _useGravity;
        protected Coroutine _currentTimer;

        public Action OnTrigger;

        //public bool Ready { get; protected set; }
        public float Mass { get => rigidbody.mass; }
        public float Gravity { get => _baseGravity; }

        public void Init(Transform barrel, int damage, float speed)
        {
            SetActive(false);
            _barrel = barrel;
            _damage = damage;
            _speed = speed;
            rigidbody = GetComponent<Rigidbody>();
            //_baseGravity = rigidbody.gravityScale;
            _useGravity = rigidbody.useGravity;
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
                rigidbody.useGravity = _useGravity;
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

            transform.position = _barrel.position;
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            //rigidbody.angularVelocity = 0;
            transform.rotation = Quaternion.identity;

            //transform.SetParent(_barrel);
            
            //Ready = true;
        }


        protected override void OnCollision(IInteractionResponder interactor)
        {
            if ((isFromPlayer && !interactor.IsPlayer)
                || (!isFromPlayer && interactor.IsPlayer))
                interactor.ReceiveDamage(_damage);
        }

        protected override void OnAnyContact(Transform collided) => Destroy();

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
            SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }
}