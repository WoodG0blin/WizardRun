using JoostenProductions;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IView : IDisposable
    {
        Vector3 Position { get; }
        float XDirection { get; }
        Collider2D collider { get; }
        SpriteRenderer renderer { get; }
        Rigidbody2D rigidbody { get; }
        Transform visualBody { get; }

        void SetActive(bool active);
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        void SetDirection(float direction);
        ContactsPuller AccessContacts();
    }

    internal interface IAnimated : IView
    {
        ActionState animationState { get; set; }
        void InitiateAnimations(AnimationSequence[] animations);
    }

    internal interface IInteractive : IView
    {
        event Action<IInteractive> onTrigger;
        void Interact(Controller target);
    }

    internal abstract class View : MonoBehaviour, IView
    {
        private SpriteRenderer _renderer;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Transform _visualBody;

        private Vector3 _initialScale = Vector3.zero;

        private int _xDirection = 1;

        private ContactsPuller _contacts;

        private bool _subscribedToUpdate = false;
        private bool _disposed = false;

        public Vector3 Position { get => transform.position; }
        public float XDirection { get => _xDirection; }
        new public SpriteRenderer renderer
        {
            get
            {
                if (!_renderer)
                    if (!visualBody.TryGetComponent<SpriteRenderer>(out _renderer)) _renderer = visualBody.AddComponent<SpriteRenderer>();
                return _renderer;
            }
            private set => _renderer = value;
        }

        new public Rigidbody2D rigidbody
        {
            get
            {
                if (!_rigidbody)
                    if (!TryGetComponent<Rigidbody2D>(out _rigidbody)) _rigidbody = transform.AddComponent<Rigidbody2D>();
                return _rigidbody;
            }
            private set => _rigidbody = value;
        }
        new public Collider2D collider
        {
            get
            {
                if (!_collider)
                    if (!TryGetComponent<Collider2D>(out _collider)) _collider = transform.AddComponent<CircleCollider2D>();
                return _collider;
            }
            private set => _collider = value;
        }

        public Transform visualBody
        {
            get
            {
                if (!_visualBody)
                    _visualBody = transform.Find("VisualBody") ?? transform;
                return _visualBody;
            }
            private set => _visualBody = value;
        }

        public void SetActive(bool active) => gameObject.SetActive(active);
        public void SetPosition(Vector3 position) => transform.position = position;
        public void SetRotation(Quaternion rotation) => transform.rotation = rotation;
        public void SetDirection(float direction)
        {
            if (_initialScale == Vector3.zero) _initialScale = visualBody.localScale;
            _xDirection = direction > 0 ? 1 : -1;
            visualBody.localScale = new Vector3(_xDirection * _initialScale.x, _initialScale.y, _initialScale.z);
        }

        public ContactsPuller AccessContacts()
        {
            _contacts ??= new ContactsPuller(collider);
            _contacts.Update();
            return _contacts;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;

            OnDestruction();
            UnRegisterFromUpdate();
            _disposed = true;
            GameObject.Destroy(gameObject);
        }

        protected virtual void OnDestruction() { }

        protected void RegisterOnUpdate()
        {
            UpdateManager.SubscribeToUpdate(OnUpdate);
            _subscribedToUpdate= true;
        }

        protected void UnRegisterFromUpdate()
        {
            if(_subscribedToUpdate) UpdateManager.UnsubscribeFromUpdate(OnUpdate);
            _subscribedToUpdate= false;
        }
        protected virtual void OnUpdate() { }
    }
}
