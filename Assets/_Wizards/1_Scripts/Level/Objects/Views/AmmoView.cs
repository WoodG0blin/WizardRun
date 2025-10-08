using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    public class AmmoView : LevelObjectView
    {
        [SerializeField] protected float lifetime = 5.0f;

        protected Action<float> moveMethod;

        protected bool isFromPlayer;

        protected int damage;

        protected bool collided = false;
        protected bool finish = false;

        protected float horizontalVelocity;
        protected float verticalVelocity;

        protected float distanceAccount;

        public void Init(int damage, AmmoType type, bool fromPlayer)
        {
            this.damage = damage;

            isFromPlayer = fromPlayer;

            moveMethod = type switch
            {
                AmmoType.Ballistic => BallisticMove,
                AmmoType.Explosion => ExplosiveMove,
                _ => DirectMove
            };

            SetActive(false);

            transform.rotation = Quaternion.identity;
        }

        public void Fire(Vector2 direction, float distance = 0)
        {
            transform.SetParent(null);
            SetActive(true);

            StartCoroutine(Move(direction, distance));
        }


        protected override void OnCollision(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer ^ isFromPlayer)
            {
                interactor.KickOff(0.2f);
                interactor.ReceiveDamage(damage);
                SetActive(false);
            }
        }
        protected override void OnAnyContact(Transform collided) => this.collided = true;

        private IEnumerator Move(Vector2 direction, float range)
        {
            horizontalVelocity = direction.x;
            verticalVelocity = direction.y;

            float timer = 0;
            float distance = -1;

            while(timer < lifetime && distance < range && !finish)
            {
                moveMethod?.Invoke(Time.deltaTime);
                timer += Time.deltaTime;
                if (range > 0) distance = distanceAccount;
                yield return null;
            }

            Destroy();
        }

        private void DirectMove(float deltaTime)
        {
            finish = collided;
            if (!finish)
            {
                transform.position += new Vector3(horizontalVelocity, verticalVelocity, 0) * deltaTime;
                distanceAccount += horizontalVelocity * deltaTime;
            }
        }

        private void BallisticMove(float deltaTime)
        {
            finish = collided;
            if (!finish)
            {
                transform.position += new Vector3(horizontalVelocity, verticalVelocity, 0) * deltaTime;
                verticalVelocity -= 9.81f * deltaTime;
                distanceAccount += horizontalVelocity * deltaTime;
            }
        }

        private void ExplosiveMove(float deltaTime)
        {
            finish = false;
            transform.localScale += Vector3.one * deltaTime*10;
            distanceAccount += deltaTime * 10;
        }


        protected void Destroy()
        {
            //if (_currentTimer != null)
            //{
            //    StopCoroutine(_currentTimer);
            //    _currentTimer = null;
            //}
            SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }
}