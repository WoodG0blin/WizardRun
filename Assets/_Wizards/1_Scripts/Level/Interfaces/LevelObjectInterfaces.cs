using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IPlayerPositionObserver
    {
        void SetNewPlayerPosition(Vector3 playerPosition);
    }

    internal interface IPortal
    {
        Action onPortalEnter { get; set; }
    }

    internal interface IBonusGenerator
    {
        Action<BonusType, int> OnBonusCollect { get; set; }
    }
}