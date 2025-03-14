using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface ILevelObjectView
    {
        void Draw(Vector3 position);
        void SetActive(bool active);

        void SetUpdateActions(Action onUpdate);
        void FinishInitiation();

        Vector3 Position { get; }

        public ViewMover Mover { get; }
        public IJump Jumper { get; }

        IContactsPuller AccessContacts();
    }
}