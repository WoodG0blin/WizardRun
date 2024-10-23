using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface ICameraView
    {
        void SetNewTargetPosition(float targetX, float targetY);
    }

    internal class CameraView : MonoBehaviour, ICameraView
    {
        private float _targetX;
        private float _targetY;
        private float _speed = 2f;

        private Vector3 _cameraPosition;

        //private float _offset = 0.5f;
        //private float _offsetThreshold = 2f;

        private BackGroundManager _backGround;
        [SerializeField] private Transform[] _backGrounds;

        private void Awake()
        {
            _cameraPosition= transform.position;
            _backGround = new BackGroundManager(transform, _backGrounds);
        }

        public void Init(float speed, Transform[] backGrounds)
        {
            _cameraPosition = transform.position;
            _speed = speed;
            _backGrounds = backGrounds;
            _backGround = new BackGroundManager(transform, _backGrounds);
        }

        public void SetNewTargetPosition(float targetX, float targetY)
        {
            _targetX = targetX;
            _targetY = targetY;
        }

        private void Update()
        {
            //_targetX = _player.transform.position.x;
            //if (Mathf.Abs(_player.rigidbody.velocity.x) > _offsetThreshold) _targetX += _offset * Mathf.Sign(_player.rigidbody.velocity.x);

            //_targetY = _player.transform.position.y;
            //if (Mathf.Abs(_player.rigidbody.velocity.y) > _offsetThreshold) _targetY += _offset * Mathf.Sign(_player.rigidbody.velocity.y);
            //_targetY = Mathf.Clamp(_targetY, -3.5f, 3.5f);

            Vector3 oldPosition = _cameraPosition;
            _cameraPosition = Vector3.Lerp(_cameraPosition, new Vector3(_targetX, _targetY, _cameraPosition.z), Time.deltaTime * _speed);
            transform.position = _cameraPosition;
            _backGround.Update(_cameraPosition - oldPosition);
        }
    }
}
