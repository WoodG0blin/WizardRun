using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WizardsPlatformer
{
    internal class PumpkinView : AmmoView
    {
        private Vector2 _directionOffset;
        private int _targetWaitTime = 2;

        protected override void OnFire(Vector2 direction)
        {
            StartCoroutine(DelayedFire(direction, Random.Range(_targetWaitTime - 1f, _targetWaitTime + 1f)));
        }


        private IEnumerator DelayedFire(Vector2 direction, float seconds)
        {
            yield return new WaitForSeconds(seconds);

            _directionOffset = Vector3.right * Random.Range(-0.2f, 0.2f);

            rigidbody.gravityScale = 1;
            rigidbody.AddForce(direction * _speed + _directionOffset, ForceMode2D.Impulse);
            rigidbody.AddTorque(_speed * 10f, ForceMode2D.Impulse);
        }
    }
}