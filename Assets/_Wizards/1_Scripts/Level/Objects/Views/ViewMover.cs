using System;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IViewMover
    {
        void Update(float deltaTime);
        Vector2 Velocity { get; }

        void SetInput(Vector2 direction, float speed = 1);

        void Jump(float force);
        bool IsGrounded { get; }
        bool IsExecutingJump { get; }

        void GetKickOff(float force);

        MovementType Type { get; }
        Action<MovementType> OnRequestReset { get; set; }
    }

    public enum MovementType
    {
        None = 0,
        Simple = 1,
        Flying = 10
    }


    public class ViewMover : IViewMover
    {
        protected CharacterController characterController;
        protected Collider collider;

        protected const float GRAVITY = 9.81f;
        protected const float BASE_MOVE_SPEED = 2f;
        protected const float FLUCTUATION_TIME = 0.2f;
        protected const float STOP_TIME = 0.2f;
        protected const float MOVE_THRESHOLD = 0.005f;

        protected bool isControlled = true;

        protected float groundedTimer;
        protected float jumpTimer;
        protected Vector2 moveInput;
        protected float jumpImpulseInput;

        protected Transform _transform;

        public bool IsGrounded => groundedTimer > 0;
        public bool IsExecutingJump => moveInput.y > MOVE_THRESHOLD;
        public Vector2 Velocity => moveInput;

        public Action<MovementType> OnRequestReset { get; set; }
        public MovementType Type { get; protected set; }

        internal ViewMover(Transform levelObject)
        {
            _transform = levelObject;
            Type = MovementType.Simple;

            if (!_transform.TryGetComponent<CharacterController>(out characterController)) characterController = _transform.AddComponent<CharacterController>();
        }

        public virtual void Update(float deltaTime)
        {
            if (groundedTimer > 0) groundedTimer -= deltaTime; //allowance for grounded fluctuations of less than FLUCTUATION TIME
            if (jumpTimer > 0) jumpTimer -= deltaTime;

            if (characterController.isGrounded)
            {
                groundedTimer = FLUCTUATION_TIME;
                if (moveInput.y < 0) moveInput.y = 0;
                isControlled = true;
            }

            moveInput.y -= GRAVITY * deltaTime;

            //FaceForward if not in uncontrolled kickOff & with enough movement force
            if (isControlled && Mathf.Abs(moveInput.x) > MOVE_THRESHOLD)
                _transform.forward = (Vector3.forward * moveInput.x).normalized;

            if (jumpTimer > 0)
            {
                moveInput.y += jumpImpulseInput;

                groundedTimer = 0;
                jumpTimer = 0;
                jumpImpulseInput = 0;
            }

            characterController.Move(new Vector3(moveInput.x, moveInput.y, 0) * deltaTime);

            AdjustToStop(ref moveInput.x, deltaTime / STOP_TIME);
        }

        protected void AdjustToStop(ref float input, float deltaCoeff)
        {
            float absSpeed = Mathf.Abs(input);
            absSpeed -= absSpeed * deltaCoeff;
            input = Mathf.Sign(input) * Mathf.Clamp(absSpeed, 0, absSpeed);
        }

        public virtual void SetInput(Vector2 direction, float speed = 1)
        {
            if (isControlled && Mathf.Abs(direction.x) > MOVE_THRESHOLD)
            {
                moveInput.x = Mathf.Clamp(direction.x, -1, 1) * BASE_MOVE_SPEED * speed;
            }
        }

        public void Jump(float speed)
        {
            if (isControlled)
            {
                jumpImpulseInput = Mathf.Sqrt(2 * speed * BASE_MOVE_SPEED * GRAVITY);
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

    public class FlyingViewMover : ViewMover
    {
        public FlyingViewMover(Transform levelObject) : base(levelObject) { Type = MovementType.Flying; }

        public override void Update(float deltaTime)
        {
            if (groundedTimer > 0) groundedTimer -= deltaTime; //allowance for grounded fluctuations of less than FLUCTUATION TIME
            if (jumpTimer > 0) jumpTimer -= deltaTime;

            if (characterController.isGrounded)
            {
                groundedTimer = FLUCTUATION_TIME;
                if (moveInput.y < 0) moveInput.y = 0;
                isControlled = true;
            }

            //FaceForward if not in uncontrolled kickOff & with enough movement force
            if (isControlled && Mathf.Abs(moveInput.x) > MOVE_THRESHOLD)
                _transform.forward = (Vector3.forward * moveInput.x).normalized;


            characterController.Move(new Vector3(moveInput.x, moveInput.y, 0) * deltaTime);

            AdjustToStop(ref moveInput.x, deltaTime / STOP_TIME);
            AdjustToStop(ref moveInput.y, deltaTime / STOP_TIME);
        }
        public override void SetInput(Vector2 direction, float speed = 1)
        {
            if (isControlled)
            {
                if(Mathf.Abs(direction.x) > MOVE_THRESHOLD) moveInput.x = Mathf.Clamp(direction.x, -1, 1) * BASE_MOVE_SPEED * speed;
                if (Mathf.Abs(direction.y) > MOVE_THRESHOLD) moveInput.y = Mathf.Clamp(direction.y, -1, 1) * BASE_MOVE_SPEED * speed;
            }
        }
    }
}