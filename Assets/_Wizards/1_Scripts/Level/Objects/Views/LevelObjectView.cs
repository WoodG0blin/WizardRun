using System;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelObjectView : MonoBehaviour, ILevelObjectView
    {
        [SerializeField] public string Message;

        private Transform _visualBody;
        private ContactsPuller3D _contacts;
        private AnimationController _animator;

        private Action _onUpdateAction;

        protected bool initiated = false;
        protected float kickCoeff = 2f;

        public IViewMover Mover { get; set; }
        public IInteractionResponder InteractionResponder { get; set; }
        public Action<IInteractionResponder> OnInteraction { get; set; }

        [field: SerializeField] public Vector3 Position => transform.position;
        public float XDirection { get; protected set; }
        //public float XDirection { get => Mathf.Sign(transform.right.x); }

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

        protected Transform visualBody
        {
            get
            {
                if (!_visualBody)
                    //_visualBody = transform.Find("VisualBody") ?? transform;
                    _visualBody = transform;
                return _visualBody;
            }
            private set => _visualBody = value;
        }


        public void Draw(Vector3 position)
        {
            transform.position = position;
            SetActive(true);
        }
        public void SetActive(bool active) => gameObject.SetActive(active);


        public void SetUpdateActions(Action onUpdate) => _onUpdateAction = onUpdate;


        public void FinishInitiation()
        {
            SetMover(MovementType.None);
            OnInitiation();
            initiated = true;
        }
        protected void SetMover(MovementType type)
        {
            Mover = type switch
            {
                MovementType.Simple => new ViewMover(visualBody),
                MovementType.Flying => new FlyingViewMover(visualBody),
                _ => null
            };
            if (Mover != null) Mover.OnRequestReset = SetMover;
        }
        protected virtual void OnInitiation() { }


        public virtual void SetTargetDirection(Vector3 direction) => XDirection = (direction - Position).x > 0 ? 1 : -1;

        
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


        public void Destroy() => GameObject.Destroy(gameObject);
    }
}