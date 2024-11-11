using System;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class LevelObjectView : MonoBehaviour, ILevelObjectView
    {
        private SpriteRenderer _renderer;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Transform _visualBody;

        private ContactsPuller _contacts;

        private Vector3 _initialScale = Vector3.zero;

        private int _xDirection = 1;


        protected bool initiated = false;
        protected LevelObjectConfig config;
        protected float kickCoeff = 2f;

        public IInteractionResponder InteractionResponder { get; set; }

        public Vector3 Position { get => transform.position; }
        public float XDirection { get => _xDirection; }

        public Action<IInteractionResponder> OnInteraction { get; set; }

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


        public void Init(LevelObjectConfig config)
        {
            this.config = config;
            OnInit();
        }
        protected virtual void OnInit() { }


        private void Update()
        {
            if (initiated) OnUpdate();
        }
        protected virtual void OnUpdate() { }


        public void Draw(Vector3 position)
        {
            SetPosition(position);
            SetActive(true);
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


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent(out LevelObjectView interactor))
            {
                var target = interactor.InteractionResponder;
                if (target != null) OnCollision(target);
            }
            OnAnyContact(collision.transform);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out LevelObjectView interactor))
            {
                var target = interactor.InteractionResponder;
                if (target != null) OnCollision(target);
            }
            OnAnyContact(collision.transform);
        }

        protected virtual void OnCollision(IInteractionResponder interactor) { }
        protected virtual void OnAnyContact(Transform collided) { }
        public void FinishInitiation() => initiated = true;
    }
}