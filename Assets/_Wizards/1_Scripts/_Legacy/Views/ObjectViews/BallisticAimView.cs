using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class BallisticAimView : AimView
    {
        private float _speed;
        private float G;
        private int _minAngle;

        public void Init(float maxDistance, float bulletSpeed, float gravityCoeff, int minAngle = 45)
        {
            _speed= bulletSpeed;
            G =gravityCoeff * 9.8f;
            _minAngle = minAngle;
            base.Init(maxDistance);
        }

        protected override (float angle, Vector3 axis) CalculateRotationParameters()
        {
            float _angle = 0f;

            for (int i = 89; i > _minAngle; i--)
            {
                float dx = Mathf.Abs(_playerPosition.x - _position.x);
                float targetApprox = CalcBallisticDY(dx, i) + _position.y;

                if (Mathf.Abs(targetApprox - _playerPosition.y) < 0.5f)
                {
                    _angle = (90 - i) * (int)Mathf.Sign(_playerPosition.x - _position.x);
                    break;
                }
            }

            return (_angle, Vector3.back);
        }

        private float CalcBallisticDY(float dx, float angle)
        {
            float tan = Mathf.Tan(RAD(angle));
            float cos = Mathf.Cos(RAD(angle));

            return dx * tan - G * (dx * dx) / (2 * _speed * _speed * cos * cos);
        }

        private float RAD(float angle) => Mathf.Deg2Rad * angle;
    }
}