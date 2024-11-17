using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsPlatformer;

namespace WizardsPlatformer
{
    internal class PortalView : LevelObjectView
    {

        void Update() => animator.UpdateValues(Vector3.zero);

        protected override void OnCollision(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                OnInteraction?.Invoke(interactor);
                SetActive(false);
            }
        }
    }
}