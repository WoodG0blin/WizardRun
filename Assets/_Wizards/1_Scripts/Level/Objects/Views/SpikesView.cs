using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class SpikesView : LevelObjectView
    {
        protected override void OnCollision(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                SetActive(false);
                OnInteraction?.Invoke(interactor);
            }
        }
    }
}