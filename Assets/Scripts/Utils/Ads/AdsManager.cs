using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WizardsPlatformer
{
    //    internal class AdsManager : SingletonMonobehaviour<AdsManager>, IUnityAdsInitializationListener, IAdsService
    //    {
    //        private string _gameID = "5169447";

    //        [Header("ACTIVATION")]
    //        [SerializeField] private bool _showTestAds = false;

    //        [Header("COMPONENTS")]
    //        [SerializeField] private bool _interstitial;
    //        [SerializeField] private string _interstitialID;
    //        [SerializeField] private bool _rewarded;
    //        [SerializeField] private string _rewardedID;
    //        [SerializeField] private bool _banner;
    //        [SerializeField] private string _bannerID;

    //        [field: Header("EVENTS")]
    //        [field: SerializeField] public UnityEvent OnInitialized { get; private set; }

    //        public IAdsShower InterstitialPlayer { get; private set; }
    //        public IAdsShower RewardedPlayer { get; private set; }
    //        public IAdsShower BannerPlayer { get; private set; }
    //        public bool IsInitialized => Advertisement.isInitialized;

    //        private void Awake()
    //        {
    //            Advertisement.Initialize(_gameID, testMode: true, enablePerPlacementLoad: true, this);

    //            if (_showTestAds)
    //            {
    //                InterstitialPlayer = new AdsShower(_interstitial ? _interstitialID : "");
    //                RewardedPlayer = new AdsShower(_rewarded ? _rewardedID : "");
    //                BannerPlayer = new AdsShower(_banner ? _bannerID : "");
    //            }
    //            else
    //            {
    //                InterstitialPlayer = new StubAdsShower();
    //                RewardedPlayer = new StubAdsShower();
    //                BannerPlayer = new StubAdsShower();
    //            }
    //        }

    //        void IUnityAdsInitializationListener.OnInitializationComplete()
    //        {
    //            Debug.Log("Ads initialization complete");
    //            OnInitialized?.Invoke();
    //        }
    //        void IUnityAdsInitializationListener.OnInitializationFailed(UnityAdsInitializationError error, string message) => Debug.Log("Ads initialization error: " + message);
    //    }
}
