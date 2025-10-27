using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class ShopView : MenuPanelView
    {
        [SerializeField] private MenuPanelView _shop;
        [SerializeField] private RewardsView _rewards;
        [SerializeField] private MenuPanelView _specials;

        [Space(10)]
        [SerializeField] private List<ShopItemConfig> _shopItems;
        [SerializeField] private ItemSlotView _itemSlotPrefab;
        [SerializeField] private Transform _shopContainer;

        private MenuPanelsManager _subPanelsManager;

        protected override void OnInit()
        {
            _subPanelsManager = new(
                panels: new() { _shop, _rewards, _specials },
                input: menuInfo);

            _subPanelsManager.ActivatePanel(_shop);
        }

        protected override void OnActivation()
        {
            for (int i = _shopContainer.childCount - 1; i >= 0; i--)
                GameObject.Destroy(_shopContainer.GetChild(i).gameObject);

            foreach (var itemConfig in _shopItems)
            {
                var itemView = Instantiate(_itemSlotPrefab, _shopContainer);
                Action click = (menuInfo.PlayerModel.Bonuses[BonusType.Coin] >= itemConfig.Price) ? () => OnProductBuy(itemConfig) : null;
                itemView.Init(itemConfig, click);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_shopContainer.GetComponent<RectTransform>());
        }


        public void OnProductBuy(ShopItemConfig item)
        {
            menuInfo.PlayerModel.AddBonus(BonusType.Coin, (int)-item.Price);

            if (item.Item != null)
            {
                Debug.Log($"Adding item {item.Item.Name}");
                menuInfo.PlayerModel.AddArtifact((ItemConfig)item.Item);
            }
            else
            {
                Debug.Log($"Adding bonus {item.Bonus.Type} {item.Bonus.Value}");
                menuInfo.PlayerModel.AddBonus((BonusType)item.Bonus.Type, (int)item.Bonus.Value);
            }
            SetActive(true);
            //Analytics.Transaction(product.definition.id, 1, product.metadata.isoCurrencyCode);
        }
    }
}
