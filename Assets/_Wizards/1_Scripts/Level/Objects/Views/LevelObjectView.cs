using System;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelObjectView : MonoBehaviour, ILevelObjectView
    {
        private Transform _visualBody;

        private ContactsPuller3D _contacts;

        private Action _onUpdateAction;

        public IViewMover Mover { get; set; }


        protected bool initiated = false;
        private AnimationController _animator;
        protected float kickCoeff = 2f;

        public IInteractionResponder InteractionResponder { get; set; }

        public Vector3 Position { get => transform.position; }
        public float XDirection { get => Mathf.Sign(transform.right.x); }

        public Action<IInteractionResponder> OnInteraction { get; set; }


        protected AnimationController animator
        {
            get
            {
                if (_animator == null)
                {
                    _animator = transform.GetComponentInChildren<AnimationController>();
                    if (_animator == null) _animator = transform.AddComponent<AnimationController>();
                    _animator.Init();
                }
                return _animator;
            }
        }

        public Transform visualBody
        {
            get
            {
                if (!_visualBody)
                    //_visualBody = transform.Find("VisualBody") ?? transform;
                    _visualBody =  transform;
                return _visualBody;
            }
            private set => _visualBody = value;
        }

        public void Draw(Vector3 position)
        {
            transform.position = position;
            SetActive(true);
        }
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }


        public void SetUpdateActions(Action onUpdate) => _onUpdateAction = onUpdate;
        public void FinishInitiation()
        {
            OnInitiation();
            initiated = true;
        }
        protected virtual void OnInitiation() { }

        private void Update()
        {
            if (initiated)
            {
                Mover?.Update(Time.deltaTime);
                OnUpdate();
                _onUpdateAction?.Invoke();
            }
        }
        protected virtual void OnUpdate() { }


        public IContactsPuller AccessContacts()
        {
            _contacts ??= new ContactsPuller3D(visualBody);
            _contacts.Update();
            return _contacts;
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out LevelObjectView interactor))
            {
                var target = interactor.InteractionResponder;
                if (target != null) OnCollision(target);
            } 
            OnAnyContact(collision.transform);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.TryGetComponent(out LevelObjectView interactor))
            {
                var target = interactor.InteractionResponder;
                if (target != null) OnCollision(target);
            }
            OnAnyContact(collision.transform);
        }

        protected virtual void OnCollision(IInteractionResponder interactor) => OnInteraction?.Invoke(interactor);
        protected virtual void OnAnyContact(Transform collided) { }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}