using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface ILiftView : ILevelObjectView
    {
        void Init(int deltaHeight);
    }

    internal class LiftView : LevelObjectView, ILiftView
    {
        private bool _goingUp;
        private float _minY;
        private float _maxY;

        public void Init(int deltaHeight)
        {
            Vector3 position = transform.position;
            _minY = Mathf.Min(position.y, position.y + deltaHeight);
            _maxY = Mathf.Max(position.y, position.y + deltaHeight);
            _goingUp = Mathf.Approximately(position.y, _minY);
            RegisterOnUpdate();
        }

        protected override void OnUpdate()
        {
            float positionY = transform.position.y;

            rigidbody.velocity = new Vector2(0, (_goingUp ? 1 : -1));

            if (_goingUp && positionY > _maxY) _goingUp = false;
            if (!_goingUp && positionY < _minY) _goingUp = true;
        }
    }
}