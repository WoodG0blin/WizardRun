using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class SpikesView : LevelObjectView
    {
        private float _damage;
        private bool _used = false;

        public void Init(float damage)
        {
            activeResponse = true;
            _damage = damage;
        }

        protected override void OnCollision(Collision2D collision)
        {
            activeResponse = false;
            //TODO animate and change image to broken spikes
            transform.gameObject.SetActive(false);
        }
    }
}