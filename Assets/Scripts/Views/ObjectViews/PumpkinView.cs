using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WizardsPlatformer
{
    internal class PumpkinView : AmmoView
    {
        private Vector2 _directionOffset;
        protected override void OnFire(Vector2 direction)
        {
            _directionOffset = Vector3.right * Random.Range(-0.2f, 0.2f);

            rigidbody.gravityScale = 1;
            rigidbody.AddForce(direction * _speed + _directionOffset, ForceMode2D.Impulse);
            rigidbody.AddTorque(_speed * 10f, ForceMode2D.Impulse);
        }

        protected override void OnTriggerExtention(IDamagable target, string tag)
        {
            if (tag.Equals("Player")) target.ReceiveDamage(_damage);
        }
    }
}