using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class LiftView : LevelObjectView
    {
        private bool _goingUp;
        private float _minY;
        private float _maxY;

        private new Rigidbody rigidbody;

        public void Init(int deltaHeight)
        {
            rigidbody= transform.GetComponent<Rigidbody>();

            Vector3 position = transform.position;
            _minY = Mathf.Min(position.y, position.y + deltaHeight);
            _maxY = Mathf.Max(position.y, position.y + deltaHeight);
            _goingUp = Mathf.Approximately(position.y, _minY);
        }

        void Update()
        {
            float positionY = transform.position.y;

            rigidbody.velocity = new Vector2(0, (_goingUp ? 1 : -1));

            if (_goingUp && positionY > _maxY) _goingUp = false;
            if (!_goingUp && positionY < _minY) _goingUp = true;
        }
    }
}