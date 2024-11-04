using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public class ContactsPuller
    {
        private Collider2D _collider;
        private ContactPoint2D[] _contacts = new ContactPoint2D[5];
        private float _collisionThreshhold = 0.9f;

        public bool HasContactDown { get; private set; }
        public bool HasContactRight { get; private set; }
        public bool HasContactLeft { get; private set; }

        public ContactsPuller(Collider2D collider)
        {
            _collider = collider;
        }

        public void Update()
        {
            HasContactDown = false;
            HasContactLeft = false;
            HasContactRight = false;

            for (int i = 0; i < _collider.GetContacts(_contacts); i++)
            {
                if (_contacts[i].normal.y > _collisionThreshhold) HasContactDown = true;
                if (_contacts[i].normal.x > _collisionThreshhold) HasContactLeft = true;
                if (_contacts[i].normal.x < -_collisionThreshhold) HasContactRight = true;
            }
        }

        public void Dispose()
        {
            _collider = null;
            _contacts = null;
        }
    }
}
