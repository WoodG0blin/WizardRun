using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using System;

namespace WizardsPlatformer
{
    internal class ShopView : MenuPanelView
    {
        [SerializeField] private MenuPanelView _shop;
        [SerializeField] private RewardsView _rewards;
        [SerializeField] private MenuPanelView _specials;

        [SerializeField] private Button _showAdButton;

        private MenuPanelsManager _subPanelsManager;

        protected override void OnInit()
        {
            _subPanelsManager = new(
                panels: new() { _shop, _rewards, _specials },
                input: menuInfo);

            _subPanelsManager.ActivatePanel(_shop);

            _rewards.OnRewardCollect += CollectedReward;
        }

        private void CollectedReward(BonusType type, int value)
        {
            Debug.Log($"Collected {type} {value}");
            //set reward through menuInfo
        }

        public void OnProductBuy(Product product)
        {
            Debug.Log($"Product {product.definition.id} is purchased");
            //Analytics.Transaction(product.definition.id, 1, product.metadata.isoCurrencyCode);
        }
    }
}
