using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    //internal class Portal : InteractableObject
    //{
    //    public Action OnPortalEnter;

    //    public Portal(LevelObjectConfig config, Vector2Int positionOnElement) : base(config, positionOnElement) { }
    //    public Portal(GameObject prefab, Vector2Int positionOnElement) : base(null, positionOnElement)
    //    {
    //        Prefab = prefab;
    //    }

    //    //public override void SetSubscriptions(ILevelEventAccounter subscriber)
    //    //{
    //    //    OnPortalEnter = subscriber.SetLevelCleared;
    //    //}

    //    protected override void ActionsOnInteraction(IInteractionResponder interactor)
    //    {
    //        if (interactor.IsPlayer)
    //        {
    //            view.SetActive(false);
    //            OnPortalEnter?.Invoke();
    //        }
    //    }

    //    protected override LevelObjectView SetView(GameObject gameObject) =>
    //        gameObject.AddComponent<LevelObjectView>();

    //    protected override void OnInitiateView()
    //    {
    //        base.OnInitiateView();
    //        view.FinishInitiation();
    //    }
    //}
}
