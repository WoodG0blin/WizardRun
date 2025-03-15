using System;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    //internal class LevelObjectViewLegacy : MonoBehaviour
    //{
    //    private Rigidbody _rigidbody;
    //    private Collider _collider;
    //    private Transform _visualBody;

    //    private IContactsPuller _contacts;

    //    private Vector3 _initialScale = Vector3.zero;

    //    private int _xDirection = 1;

    //    protected bool initiated = false;
    //    private AnimationController _animator;
    //    protected float kickCoeff = 2f;

    //    public IInteractionResponder InteractionResponder { get; set; }

    //    public Vector3 Position { get => transform.position; }
    //    public float XDirection { get => _xDirection; }

    //    public Action<IInteractionResponder> OnInteraction { get; set; }


    //    protected AnimationController animator
    //    {
    //        get
    //        {
    //            if (_animator == null)
    //            {
    //                _animator = transform.GetComponentInChildren<AnimationController>();
    //                if (_animator == null) _animator = transform.AddComponent<AnimationController>();
    //                _animator.Init();
    //            }
    //            return _animator;
    //        }
    //    }

    //    new public Rigidbody rigidbody
    //    {
    //        get
    //        {
    //            if (!_rigidbody)
    //                if (!TryGetComponent<Rigidbody>(out _rigidbody)) _rigidbody = transform.AddComponent<Rigidbody>();
    //            return _rigidbody;
    //        }
    //        private set => _rigidbody = value;
    //    }
    //    new public Collider collider
    //    {
    //        get
    //        {
    //            if (!_collider)
    //                if (!TryGetComponent<Collider>(out _collider)) _collider = transform.AddComponent<CapsuleCollider>();
    //            return _collider;
    //        }
    //        private set => _collider = value;
    //    }

    //    public Transform visualBody
    //    {
    //        get
    //        {
    //            if (!_visualBody)
    //                //_visualBody = transform.Find("VisualBody") ?? transform;
    //                _visualBody =  transform;
    //            return _visualBody;
    //        }
    //        private set => _visualBody = value;
    //    }


    //    private void Update()
    //    {
    //        if (initiated) OnUpdate();
    //    }
    //    protected virtual void OnUpdate() { }


    //    public void Draw(Vector3 position)
    //    {
    //        SetPosition(position);
    //        SetActive(true);
    //    }


    //    public void SetActive(bool active) => gameObject.SetActive(active);
    //    public void SetPosition(Vector3 position) => transform.position = position;
    //    public void SetRotation(Quaternion rotation) => transform.rotation = rotation;
    //    public void SetDirection(float direction)
    //    {
    //        if (_initialScale == Vector3.zero) _initialScale = visualBody.localScale;
    //        _xDirection = direction > 0 ? 1 : -1;
    //        visualBody.localScale = new Vector3(_xDirection * _initialScale.x, _initialScale.y, _initialScale.z);
    //    }


    //    public IContactsPuller AccessContacts()
    //    {
    //        //_contacts ??= new ContactsPuller(collider);
    //        _contacts ??= new ContactsPuller3D(visualBody);
    //        _contacts.Update();
    //        return _contacts;
    //    }


    //    //private void OnCollisionEnter2D(Collision2D collision)
    //    //{
    //    //    if (collision.transform.TryGetComponent(out LevelObjectView interactor))
    //    //    {
    //    //        var target = interactor.InteractionResponder;
    //    //        if (target != null) OnCollision(target);
    //    //    }
    //    //    OnAnyContact(collision.transform);
    //    //}

    //    //private void OnTriggerEnter2D(Collider2D collision)
    //    //{
    //    //    if (collision.transform.TryGetComponent(out LevelObjectView interactor))
    //    //    {
    //    //        var target = interactor.InteractionResponder;
    //    //        if (target != null) OnCollision(target);
    //    //    }
    //    //    OnAnyContact(collision.transform);
    //    //}

    //    private void OnCollisionEnter(Collision collision)
    //    {
    //        if (collision.transform.TryGetComponent(out LevelObjectView interactor))
    //        {
    //            var target = interactor.InteractionResponder;
    //            if (target != null) OnCollision(target);
    //        }
    //        OnAnyContact(collision.transform);
    //    }

    //    private void OnTriggerEnter(Collider collision)
    //    {
    //        if (collision.transform.TryGetComponent(out LevelObjectView interactor))
    //        {
    //            var target = interactor.InteractionResponder;
    //            if (target != null) OnCollision(target);
    //        }
    //        OnAnyContact(collision.transform);
    //    }

    //    protected virtual void OnCollision(IInteractionResponder interactor) { }
    //    protected virtual void OnAnyContact(Transform collided) { }
    //    public void FinishInitiation() => initiated = true;
    //}



    internal class LevelObjectView : MonoBehaviour, ILevelObjectView
    {
        private Rigidbody _rigidbody;
        private Transform _visualBody;

        private ContactsPuller3D _contacts;

        private Action _onUpdateAction;

        public ViewMover Mover { get; protected set; }
        public IJump Jumper { get; protected set; }


        protected bool initiated = false;
        private AnimationController _animator;
        protected float kickCoeff = 2f;

        public IInteractionResponder InteractionResponder { get; set; }

        public Vector3 Position { get => transform.position; }
        public float XDirection { get => Mathf.Sign(transform.forward.z); }

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
        new public Rigidbody rigidbody
        {
            get
            {
                //if (!_rigidbody)
                //    if (!TryGetComponent<Rigidbody>(out _rigidbody)) _rigidbody = transform.AddComponent<Rigidbody>();
                return _rigidbody;
            }
            private set => _rigidbody = value;
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
        public void SetActive(bool active) => gameObject.SetActive(active);


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

        protected virtual void OnCollision(IInteractionResponder interactor) { }
        protected virtual void OnAnyContact(Transform collided) { }
    }

    internal class ViewMover : IJump
    {
        protected CharacterController characterController;

        protected const float GRAVITY = 9.81f;
        protected const float FLUCTUATION_TIME = 0.2f;
        protected const float STOP_TIME = 0.2f;
        protected const float MOVE_THRESHOLD = 0.05f;

        protected float groundedTimer;
        protected float jumpTimer;
        protected float verticalVelocity;
        protected float horizontalInput;
        protected float jumpImpulseInput;

        //protected Rigidbody rigidbody;
        private Transform _transform;
        //private Vector3 _initialScale;

        //protected IContactsPuller contacts;

        public bool IsGrounded => groundedTimer > 0;
        public Vector2 Velocity => new(horizontalInput, verticalVelocity);

        internal ViewMover(Transform levelObject)
        {
            _transform = levelObject;

            if(!_transform.TryGetComponent<CharacterController>(out characterController)) characterController = _transform.AddComponent<CharacterController>();

            //rigidbody = _transform.GetComponent<Rigidbody>();
            //_initialScale = _transform.localScale;

            //this.contacts = contacts;
        }

        public void Update(float deltaTime)
        {
            if(groundedTimer > 0) groundedTimer -= deltaTime; //allowance for grounded fluctuations of less than FLUCTUATION TIME
            if(jumpTimer > 0) jumpTimer -= deltaTime;

            if (characterController.isGrounded)
            {
                groundedTimer = FLUCTUATION_TIME;
                if (verticalVelocity < 0) verticalVelocity = 0;
            }

            verticalVelocity -= GRAVITY * deltaTime;

            if(Mathf.Abs(horizontalInput) > characterController.minMoveDistance) _transform.forward = (Vector3.forward * horizontalInput).normalized;

            if(jumpTimer > 0)
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

        public void SetMoveTo(float xDirection, float speed = 1)
        {
            //if(groundedTimer > 0)
                horizontalInput = Mathf.Clamp(xDirection, -1, 1) * speed;
        }

        public void Jump(float force)
        {
            jumpImpulseInput = force;
            jumpTimer = FLUCTUATION_TIME;
        }

        //internal void Move(float direction)
        //{
        //    SetXDirection(direction);
        //    if (HasNoBarrier(direction))
        //        rigidbody.velocity = new(direction, rigidbody.velocity.y, 0);
        //}

        //public void SetXDirection(float xDirection)
        //{
        //    int _xDirection = xDirection > 0 ? 1 : -1;
        //    rigidbody.transform.localScale = new Vector3(_xDirection * Mathf.Abs(_initialScale.x), _initialScale.y, _initialScale.z);
        //}

        //private bool HasNoBarrier(float direction) =>
        //    (direction > 0 && !contacts.HasContactRight) || (direction < 0 && !contacts.HasContactLeft);
    }

    //internal class ViewJumper : ViewMover, IJump
    //{
    //    internal ViewJumper(Transform levelObject, IContactsPuller contacts) : base(levelObject, contacts)
    //    {
    //    }

    //    public void Jump(float force)
    //    {
    //        rigidbody.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    //    }
    //}
}