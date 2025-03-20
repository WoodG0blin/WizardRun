using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Portal : InteractableObject, IPortal
    {
        public Portal(LevelObjectConfig config, Vector2Int positionOnElement) : base(config, positionOnElement) { }

        public Action onPortalEnter { get; set; }

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                view.SetActive(false);
                onPortalEnter?.Invoke();
            }
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<LevelObjectView>();

        protected override void OnInitiateView()
        {
            base.OnInitiateView();
            view.FinishInitiation();
        }
    }
}
