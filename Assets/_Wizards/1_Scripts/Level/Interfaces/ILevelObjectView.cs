using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface ILevelObjectView
    {
        void SetPosition(Vector2 position);
        void SetActive(bool active);

        void SetUpdateActions(Action onUpdate);
        void FinishInitiation();

        Vector2 Position { get; }

        public IViewMover Mover { get; }

        void Destroy();
    }
}