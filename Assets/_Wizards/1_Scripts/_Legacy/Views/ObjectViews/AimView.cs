using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class AimView : View
    {
        protected float _maxDistance;

        protected Vector3 _playerPosition;
        protected Vector3 _position;
        protected Vector3 _aimDirection;

        public void Init(float maxDistance)
        {
            _maxDistance = maxDistance;
            _position = transform.position;
            _aimDirection = transform.up;
        }

        public void UpdateAim(Vector3 targetPosition)
        {
            _playerPosition = targetPosition;

            if (InDistance)
            {
                var parameters = CalculateRotationParameters();
                SetRotation(Quaternion.AngleAxis(parameters.angle, parameters.axis));
                _aimDirection = transform.up;
            }
        }

        public Vector3 Direction { get => _aimDirection; }

        public bool InDistance
        {
            get => _maxDistance > 0 ? Vector3.Magnitude(_playerPosition - _position) <= _maxDistance : true;
        }

        protected virtual (float angle, Vector3 axis) CalculateRotationParameters()
        {
            Vector3 towards = _playerPosition - _position;
            return (Vector3.Angle(Vector3.up, towards), Vector3.Cross(Vector3.up, towards));
        }
    }
}