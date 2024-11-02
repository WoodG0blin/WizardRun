using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IObserver 
    {
    }

    internal interface IPlayerPositionObserver
    {
        void RegisterObserveTarget(SubscribtableProperty<Vector3> observeTarget);
    }

    internal interface IPortal
    {
        Action onPortalEnter { get; set; }
    }

    internal interface IBonus
    {
        Action<BonusType, int> onBonusCollect { get; set; }
    }
}