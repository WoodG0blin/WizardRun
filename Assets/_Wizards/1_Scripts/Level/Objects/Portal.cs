using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Portal : InteractableObject
    {
        private Action onPortalEnter;

        public Portal(LevelObjectConfig config, Vector2Int positionOnElement) : base(config, positionOnElement) { }


        public override void SetSubscriptions(ILevelEventAccounter subscriber)
        {
            onPortalEnter = subscriber.SetLevelCleared;
        }

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
