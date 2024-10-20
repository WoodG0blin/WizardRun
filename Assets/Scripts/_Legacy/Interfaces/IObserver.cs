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

    internal interface ILevelStateObserver
    {
        void RegisterLevelState(SubscribtableProperty<GameState> gameState);
        event Action onPortalEnter;
    }

    internal interface IBonus
    {
        event Action<BonusType, int> onBonusCollect;
    }
}