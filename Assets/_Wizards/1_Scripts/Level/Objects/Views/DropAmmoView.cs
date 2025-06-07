using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WizardsPlatformer
{
    internal class DropAmmoView : AmmoView
    {
        private Vector2 _directionOffset;
        private int _targetWaitTime = 2;

        //protected override void OnFire(Vector2 direction)
        //{
        //    rigidbody.useGravity = false;
        //    StartCoroutine(DelayedFire(direction, Random.Range(_targetWaitTime - 1f, _targetWaitTime + 1f)));
        //}


        //private IEnumerator DelayedFire(Vector2 direction, float seconds)
        //{
        //    yield return new WaitForSeconds(seconds);

        //    _directionOffset = Vector3.right * Random.Range(-0.2f, 0.2f);

        //    rigidbody.useGravity = true;
        //    rigidbody.AddForce(direction * _speed + _directionOffset, ForceMode.Impulse);
        //    rigidbody.AddRelativeTorque(new(_speed * 10f, 0, 0), ForceMode.Impulse);
        //    //rigidbody.AddTorque(_speed * 10f, ForceMode.Impulse);
        //}

        //protected override void OnAnyContact(Transform collided)
        //{
        //    if (collided.gameObject.layer == 6) Destroy();
        //}

        //protected override void OnCollision(IInteractionResponder interactor)
        //{
        //    if ((isFromPlayer && !interactor.IsPlayer)
        //        || (!isFromPlayer && interactor.IsPlayer))
        //    {
        //        interactor.ReceiveDamage(_damage);
        //        Destroy();
        //    }
        //}
    }
}