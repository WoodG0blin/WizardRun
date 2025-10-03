using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardsPlatformer
{
    internal class BallisticShooterView : LevelObjectView
    {
        [field: SerializeField] public Transform Barrel { get; private set; }
        [SerializeField] private int _minAngle = 45;
        
        private float _fireForce;

        public void Init(float fireForce)
        {
            Barrel ??= transform;
            _fireForce = fireForce;
        }

        public Vector2 RotateBarrelTowards(Vector3 targetPosition)
        {
            var rot = CalculateRotationParameters(targetPosition);
            //Debug.Log($"Rotation parameters are: angle {rot.angle}, axis {rot.axis}");
            Barrel.rotation = Quaternion.AngleAxis(rot.angle, rot.axis);
            return new(Barrel.right.x, Barrel.right.y);
        }

        protected (float angle, Vector3 axis) CalculateRotationParameters(Vector3 targetPosition)
        {
            float _angle = 0f;

            for (int i = 89; i > _minAngle; i--)
            {
                float dx = Mathf.Abs(targetPosition.x - Position.x);
                float targetApprox = CalcBallisticDY(dx, i) + Position.y;

                if (Mathf.Abs(targetApprox - targetPosition.y) < 0.5f)
                {
                    _angle = 90 - i * (int)Mathf.Sign(targetPosition.x - Position.x);
                    break;
                }
            }

            return (_angle, Vector3.forward);
        }

        private float CalcBallisticDY(float dx, float angle)
        {
            float tan = Mathf.Tan(RAD(angle));
            float cos = Mathf.Cos(RAD(angle));

            return dx * tan -9.8f * (dx * dx) / (2 * _fireForce * _fireForce * cos * cos);
        }

        private float RAD(float angle) => Mathf.Deg2Rad * angle;

    }
}