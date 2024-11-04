using UnityEngine.Events;

namespace WizardsPlatformer
{
    internal interface IAdsService
    {
        IAdsShower InterstitialPlayer { get; }
        IAdsShower RewardedPlayer { get; }
        IAdsShower BannerPlayer { get; }
        UnityEvent OnInitialized { get; }
        bool IsInitialized { get; }
    }
}
