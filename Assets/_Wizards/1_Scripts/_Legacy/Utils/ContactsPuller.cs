using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    public class ContactsPuller : IContactsPuller
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
    }

    public class ContactsPuller3D : IContactsPuller
    {
        private Transform _owner;
        private float _verticalRaycastDistance;
        private float _horizontalRaycastDistance;
        private LayerMask _layerMask;

        public bool HasContactDown { get; private set; }
        public bool HasContactRight { get; private set; }
        public bool HasContactLeft { get; private set; }

        public ContactsPuller3D(Transform owner)
        {
            _owner = owner;
            _verticalRaycastDistance = _owner.localScale.y + 0.03f;
            _horizontalRaycastDistance = _owner.localScale.x / 2 + 0.03f;

            _layerMask = LayerMask.GetMask("Background");
        }

        public void Update()
        {
            HasContactDown = Physics.Raycast(_owner.position, Vector3.down, _verticalRaycastDistance, _layerMask);
            HasContactLeft = Physics.Raycast(_owner.position + new Vector3(0, _verticalRaycastDistance * 0.8f, 0), Vector3.left, _horizontalRaycastDistance, _layerMask)
                || Physics.Raycast(_owner.position - new Vector3(0, _verticalRaycastDistance * 0.8f, 0), Vector3.left, _horizontalRaycastDistance, _layerMask);
            HasContactRight = Physics.Raycast(_owner.position + new Vector3(0, _verticalRaycastDistance * 0.8f, 0), Vector3.right, _horizontalRaycastDistance, _layerMask)
                || Physics.Raycast(_owner.position - new Vector3(0, _verticalRaycastDistance * 0.8f , 0), Vector3.right, _horizontalRaycastDistance, _layerMask);
        }
    }
}
