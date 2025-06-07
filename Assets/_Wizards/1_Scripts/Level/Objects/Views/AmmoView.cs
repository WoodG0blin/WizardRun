using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    public class AmmoView : LevelObjectView
    {
        [SerializeField] protected float lifetime = 5.0f;

        protected Coroutine _currentTimer;


        public void Init(bool isBallistic = false)
        {
            Mover = isBallistic ? new BallisticBulletMover(visualBody) : new SimpleBulletMover(visualBody);
            
            SetActive(false);

            transform.rotation = Quaternion.identity;
        }


        public void SetMove(Vector2 direction)
        {
            transform.SetParent(null);
            SetActive(true);
            _currentTimer = StartCoroutine(DestroyAfterTime(lifetime));

            Mover.SetInput(direction);
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


    public class SimpleBulletMover : IViewMover
    {
        protected float horizontalVelocity;
        protected float verticalVelocity;

        protected Transform transform;


        public Vector2 Velocity => Vector2.zero;

        public bool IsGrounded => false;


        public SimpleBulletMover(Transform bullet)
        {
            transform = bullet;
        }


        public void Update(float deltaTime)
        {
            UpdateVelocities(deltaTime);

            transform.position += new Vector3(horizontalVelocity, verticalVelocity, 0) * deltaTime;
        }

        protected virtual void UpdateVelocities(float deltaTime) { }


        public void SetInput(Vector2 direction, float speed = 1)
        {
            horizontalVelocity = direction.x * speed;
            verticalVelocity = direction.y * speed;
        }

        public void Jump(float force) { }
        public void GetKickOff(float force) { }
    }

    public class BallisticBulletMover : SimpleBulletMover
    {
        protected const float GRAVITY = 9.81f;

        public BallisticBulletMover(Transform bullet) : base(bullet) { }


        protected override void UpdateVelocities(float deltaTime)
        {
            verticalVelocity -= GRAVITY * deltaTime;
        }
    }
}