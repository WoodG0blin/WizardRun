using System;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    public class LevelObjectView : MonoBehaviour, ILevelObjectView
    {
        [SerializeField] public string Message;

        [SerializeField] private Transform _visualBody;
        [SerializeField] private AnimationController _animator;

        private Action _onUpdateAction;

        protected bool initiated = false;
        protected float kickCoeff = 2f;

        public IViewMover Mover { get; set; }
        public IInteractionResponder InteractionResponder { get; set; }
        public Action<IInteractionResponder> OnInteraction { get; set; }

        public Vector2 Position => transform.position;
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
                }
                _animator.Init();
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


        public void SetPosition(Vector2 position)
        {
            CharacterController c;
            if (transform.TryGetComponent<CharacterController>(out c)) c.enabled = false;
            transform.localPosition = position;
            if(c != null) c.enabled = true;
            transform.rotation = Quaternion.identity;
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
                MovementType.Simple => new ViewMover(transform, SetTransformDirection),
                MovementType.Flying => new FlyingViewMover(transform, SetTransformDirection),
                _ => null
            };
            if (Mover != null) Mover.OnRequestReset = SetMover;
        }
        protected virtual void OnInitiation() { }


        public virtual void SetLookDirection(Vector2 direction)
        {
            if (Mover == null) SetTransformDirection(direction - Position);
        }

        protected void SetTransformDirection(Vector2 direction)
        {
            visualBody.right = direction.normalized;
            XDirection = visualBody.right.x > 0 ? 1 : -1;
        }

        
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