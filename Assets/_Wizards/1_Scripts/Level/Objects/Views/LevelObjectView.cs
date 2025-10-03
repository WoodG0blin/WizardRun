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

        public IViewMover Mover { get; protected set; }


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
    }

    public interface IViewMover
    {
        void Update(float deltaTime);
        Vector2 Velocity { get; }

        void SetInput(Vector2 direction, float speed = 1);

        void Jump(float force);
        bool IsGrounded { get; }
        bool IsExecutingJump { get; }

        void GetKickOff(float force);
    }

    public class ViewMover : IViewMover
    {
        protected CharacterController characterController;
        protected Collider collider;

        protected const float GRAVITY = 9.81f;
        protected const float FLUCTUATION_TIME = 0.2f;
        protected const float STOP_TIME = 0.2f;
        protected const float MOVE_THRESHOLD = 0.005f;

        protected bool isControlled = true;

        protected float groundedTimer;
        protected float jumpTimer;
        protected float verticalVelocity;
        protected float horizontalInput;
        protected float jumpImpulseInput;

        private Transform _transform;

        public bool IsGrounded => groundedTimer > 0;
        public bool IsExecutingJump => verticalVelocity > MOVE_THRESHOLD;
        public Vector2 Velocity => new(horizontalInput, verticalVelocity);

        internal ViewMover(Transform levelObject)
        {
            _transform = levelObject;

            if (!_transform.TryGetComponent<CharacterController>(out characterController)) characterController = _transform.AddComponent<CharacterController>();
        }

        public void Update(float deltaTime)
        {
            if (groundedTimer > 0) groundedTimer -= deltaTime; //allowance for grounded fluctuations of less than FLUCTUATION TIME
            if (jumpTimer > 0) jumpTimer -= deltaTime;

            if (characterController.isGrounded)
            {
                groundedTimer = FLUCTUATION_TIME;
                if (verticalVelocity < 0) verticalVelocity = 0;
                isControlled = true;
            }

            verticalVelocity -= GRAVITY * deltaTime;

            //FaceForward if not in uncontrolled kickOff & with enough movement force
            if (isControlled && Mathf.Abs(horizontalInput) > MOVE_THRESHOLD)
                _transform.forward = (Vector3.forward * horizontalInput).normalized;

            if (jumpTimer > 0)
            {
                verticalVelocity += Mathf.Sqrt(jumpImpulseInput * 2 * GRAVITY);

                groundedTimer = 0;
                jumpTimer = 0;
                jumpImpulseInput = 0;
            }

            characterController.Move(new Vector3(horizontalInput, verticalVelocity, 0) * deltaTime);

            AdjustToStop(deltaTime / STOP_TIME);
        }

        private void AdjustToStop(float deltaCoeff)
        {
            float absSpeed = Mathf.Abs(horizontalInput);
            absSpeed -= absSpeed * deltaCoeff;
            horizontalInput = Mathf.Sign(horizontalInput) * Mathf.Clamp(absSpeed, 0, absSpeed);
        }

        public void SetInput(Vector2 direction, float speed = 1)
        {
            if (isControlled)
                horizontalInput = Mathf.Clamp(direction.x, -1, 1) * speed;
        }

        public void Jump(float force)
        {
            if (isControlled)
            {
                jumpImpulseInput = force;
                jumpTimer = FLUCTUATION_TIME;
            }
        }

        public void GetKickOff(float force)
        {
            SetInput(new(-Velocity.x, 0), force * 3);
            Jump(force);
            isControlled = false;
        }
    }
}