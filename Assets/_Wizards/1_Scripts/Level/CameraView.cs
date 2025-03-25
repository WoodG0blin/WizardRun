using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal interface ICameraView
    {
        void SetNewTargetPosition(float targetX, float targetY);
        public void InitBackGrounds(Sprite[] backGroundSprites, float speed);
    }

    internal class CameraView : MonoBehaviour, ICameraView
    {
        private float _targetX;
        private float _targetY;
        private float _speed = 2f;

        private Vector3 _cameraPosition;

        private BackGroundMover _backGround;

        [SerializeField] private Transform _backGroundsContainer;
        [SerializeField] private Sprite[] _backGroundSprites;
        [SerializeField, Range(0,1)] private float _backGroundSpeedCoefficient = 1f;

        private void Awake()
        {
            _cameraPosition= transform.position;
        }

        public void InitBackGrounds(Sprite[] backGroundSprites, float speed)
        {
            _backGround = new(_backGroundSprites, _backGroundsContainer, speed);
        }

        public void SetNewTargetPosition(float targetX, float targetY)
        {
            _targetX = targetX;
            _targetY = targetY;
        }

        private void Update()
        {
            Vector3 oldPosition = _cameraPosition;
            _cameraPosition = Vector3.Lerp(_cameraPosition, new Vector3(_targetX, _targetY, _cameraPosition.z), Time.deltaTime * _speed);
            transform.position = _cameraPosition;
            _backGround?.Update((_cameraPosition - oldPosition) * _backGroundSpeedCoefficient);
        }
    }
}
