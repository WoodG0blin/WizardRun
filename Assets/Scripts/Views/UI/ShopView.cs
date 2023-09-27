using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using System;

namespace WizardsPlatformer
{
    internal interface IShopView
    {
        UnityAction OnAddRequest { set; }
        UnityAction OnReturn { set; }
        event Action<BonusType, int> OnRewardCollection;
    }

    internal class ShopView : View, IShopView
    {
        public enum Tab { shop, rewards}

        [SerializeField] private Button _backButton;
        [SerializeField] private Button _showAdButton;
        [SerializeField] private RewardsView _rewards;

        public UnityAction OnReturn { set => _backButton.onClick.AddListener(value); }
        public UnityAction OnAddRequest { set => _showAdButton.onClick.AddListener(value); }
        public event Action<BonusType, int> OnRewardCollection;

        public void OnProductBuy(Product product)
        {
            Debug.Log($"Product {product.definition.id} is purchased");
            Analytics.Transaction(product.definition.id, 1, product.metadata.isoCurrencyCode);
        }

        private void Awake()
        {
            _rewards.OnRewardCollect += CollectedReward;
        }

        private void CollectedReward(BonusType type, int value) => OnRewardCollection?.Invoke(type, value);


        protected override void OnDestruction()
        {
            _rewards.OnRewardCollect -= CollectedReward;
        }
    }
}
