using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class ScarecrowView : LevelObjectView
    {
        [field: SerializeField] public Transform Barrel { get; private set; }

        public Action<Vector3> OnFireReady { get; set; }

        public void Init()
        {
            Barrel ??= visualBody.Find("Hand");
        }

        protected override void OnUpdate()
        {
            OnFireReady?.Invoke(Vector3.up);
        }

        protected override void OnCollision(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                OnInteraction?.Invoke(interactor);
            }
        }
    }
}