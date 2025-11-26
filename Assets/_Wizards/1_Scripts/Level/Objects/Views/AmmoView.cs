using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    public class AmmoView : LevelObjectView
    {
        [SerializeField] protected float lifetime = 5.0f;

        protected Action<float> moveMethod;
        protected AmmoMover ammoMover;

        protected bool isFromPlayer;

        protected int damage;

        protected bool collided = false;

        protected float distanceAccount;

        public void Init(int damage, AmmoType type, bool fromPlayer)
        {
            this.damage = damage;

            isFromPlayer = fromPlayer;

            ammoMover = type switch
            {
                AmmoType.Ballistic => new BallisticAmmoMover(transform),
                AmmoType.Explosion => new ExplosiveAmmoMover(transform),
                _ => new DirectAmmoMover(transform)
            };

            SetActive(false);

            transform.rotation = Quaternion.identity;
        }

        public void Fire(Vector2 direction, float speed, float distance = 0)
        {
            transform.SetParent(null);
            SetActive(true);

            StartCoroutine(Move(direction, speed, distance));
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

        private IEnumerator Move(Vector2 direction, float speed, float range)
        {
            ammoMover.SetVelocities(direction, speed);

            float timer = 0;
            float distance = -1;

            while(timer < lifetime && distance < range)
            {
                if(!ammoMover.Move(collided, Time.deltaTime, ref distanceAccount)) timer = lifetime;
                timer += Time.deltaTime;
                if (range > 0) distance = distanceAccount;
                yield return null;
            }

            Destroy();
        }
    }

    public abstract class AmmoMover
    {
        protected Transform body;

        protected float horizontalVelocity;
        protected float verticalVelocity;

        public AmmoMover(Transform body) => this.body = body;

        public abstract void SetVelocities(Vector2 direction, float speed);
        public abstract bool Move(bool collided, float deltaTime, ref float distanceAccount);
    }

    public class DirectAmmoMover : AmmoMover
    {
        public DirectAmmoMover(Transform body) : base(body) { }

        public override bool Move(bool collided, float deltaTime, ref float distanceAccount)
        {
            if (!collided)
            {
                body.position += new Vector3(horizontalVelocity, verticalVelocity, 0) * deltaTime;
                distanceAccount += horizontalVelocity * deltaTime;
                return true;
            }
            return false;
        }

        public override void SetVelocities(Vector2 direction, float speed)
        {
            var vel = direction.normalized * speed;
            horizontalVelocity = vel.x;
            verticalVelocity = vel.y;
        }
    }
    public class BallisticAmmoMover : AmmoMover
    {
        private int _minAngle = 45;

        private float _fireForce = 5;

        public BallisticAmmoMover(Transform body) : base(body) { }

        public override bool Move(bool collided, float deltaTime, ref float distanceAccount)
        {
            if (!collided)
            {
                body.position += new Vector3(horizontalVelocity, verticalVelocity, 0) * deltaTime;
                verticalVelocity -= 9.81f * deltaTime;
                distanceAccount += horizontalVelocity * deltaTime;
                return true;
            }
            return false;
        }

        public override void SetVelocities(Vector2 direction, float speed)
        {
            _fireForce = speed;

            var res = CalculateRotationParameters(direction).normalized * speed;

            horizontalVelocity = res.x;
            verticalVelocity = res.y;
        }

        protected Vector2 CalculateRotationParameters(Vector2 targetPosition)
        {
            float _angle = 0f;

            for (int i = 89; i > _minAngle; i--)
            {
                float dx = Mathf.Abs(targetPosition.x);
                float targetApprox = CalcBallisticDY(dx, i);

                if (Mathf.Abs(targetApprox - targetPosition.y) < 0.5f)
                {
                    _angle = i;
                    break;
                }
            }

            return new(Mathf.Sign(targetPosition.x) * Mathf.Cos(RAD(_angle)), Mathf.Sin(RAD(_angle)));
        }

        private float CalcBallisticDY(float dx, float angle)
        {
            float tan = Mathf.Tan(RAD(angle));
            float cos = Mathf.Cos(RAD(angle));

            return dx * tan - 9.8f * (dx * dx) / (2 * _fireForce * _fireForce * cos * cos);
        }

        private float RAD(float angle) => Mathf.Deg2Rad * angle;
    }

    public class ExplosiveAmmoMover : AmmoMover
    {
        public ExplosiveAmmoMover(Transform body) : base(body) { }

        public override bool Move(bool collided, float deltaTime, ref float distanceAccount)
        {
            body.localScale += Vector3.one * deltaTime * 10;
            distanceAccount += deltaTime * 10;
            return false;
        }

        public override void SetVelocities(Vector2 direction, float speed) { }
    }
}