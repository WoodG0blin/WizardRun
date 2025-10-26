using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace WizardsPlatformer
{
    internal class ShopView : MenuPanelView
    {
        [SerializeField] private MenuPanelView _shop;
        [SerializeField] private RewardsView _rewards;
        [SerializeField] private MenuPanelView _specials;

        [Space(10)]
        [SerializeField] private List<ShopItemConfig> _shopItems;
        [SerializeField] private ShopItemView _shopItemPrefab;
        [SerializeField] private Transform _shopContainer;

        [Space(10)]
        [SerializeField] private Button _showAdButton;
        [SerializeField] private TMPro.TextMeshProUGUI _descriptionText;

        private MenuPanelsManager _subPanelsManager;

        protected override void OnInit()
        {
            InitGameShop();

            _subPanelsManager = new(
                panels: new() { _shop, _rewards, _specials },
                input: menuInfo);

            _subPanelsManager.ActivatePanel(_shop);

            _rewards.OnRewardCollect += CollectedReward;
        }

        private void InitGameShop()
        {
            foreach (var itemConfig in _shopItems)
            {
                var itemView = Instantiate(_shopItemPrefab, _shopContainer);
                itemView.Init(itemConfig, DisplayDescription, () => OnProductBuy(itemView));
            }
        }

        private void CollectedReward(BonusType type, int value)
        {
            Debug.Log($"Collected {type} {value}");
            //set reward through menuInfo
        }

        public void OnProductBuy(ShopItemView slot)
        {
            var item = slot.ItemConfig;

            Debug.Log($"Trying to buy {item.name}");

            if (menuInfo.PlayerModel.Bonuses[BonusType.Coin] >= item.Price)
            {
                menuInfo.PlayerModel.AddBonus(BonusType.Coin, -item.Price);

                if (item.Item != null)
                {
                    Debug.Log($"Adding item {item.Item.Name}");
                    menuInfo.PlayerModel.AddArtifact(item.Item);
                }
                else
                {
                    Debug.Log($"Adding bonus {item.Bonus.Type} {item.Bonus.Value}");
                    menuInfo.PlayerModel.AddBonus(item.Bonus.Type, item.Bonus.Value);
                }

                //Analytics.Transaction(product.definition.id, 1, product.metadata.isoCurrencyCode);
            }
        }

        private void DisplayDescription(ShopItemConfig item)
        {
            _descriptionText.text = item.Item != null ? item.Item.Name : $"{item.Bonus.Type} +{item.Bonus.Value}";
        }
    }
}
