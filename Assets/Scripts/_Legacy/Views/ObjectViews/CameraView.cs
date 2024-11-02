using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal interface ICameraView
    {
        void SetNewTargetPosition(float targetX, float targetY);
        public void InitBackGrounds(Sprite[] backGroundSprites);
    }

    internal class CameraView : MonoBehaviour, ICameraView
    {
        private float _targetX;
        private float _targetY;
        private float _speed = 2f;

        private Vector3 _cameraPosition;

        private BackGroundMover _backGround;
        [SerializeField] private Transform[] _backGrounds;

        private void Awake()
        {
            _cameraPosition= transform.position;
            _backGround = new BackGroundMover(_backGrounds);
        }

        public void InitBackGrounds(Sprite[] backGroundSprites)
        {
            for(int i = 0; i < Mathf.Min(_backGrounds.Length, backGroundSprites.Length); i++)
                _backGrounds[i].GetComponent<SpriteRenderer>().sprite = backGroundSprites[i];
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
            _backGround.Update(_cameraPosition - oldPosition);
        }
    }
}
