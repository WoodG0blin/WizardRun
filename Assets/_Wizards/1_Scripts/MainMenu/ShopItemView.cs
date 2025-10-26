using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class ShopItemView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private ButtonView _buyButon;
        public ShopItemConfig ItemConfig { get; private set; }

        private Action<ShopItemConfig> _displayDescription;
        private Action _buyItem;


        public void Init(ShopItemConfig item, Action<ShopItemConfig> displayDescription, Action buyItem)
        {
            ItemConfig = item;

            _icon.sprite = ItemConfig.Item == null ? ItemConfig.DisplayImage : ItemConfig.Item.Icon;
            _icon.preserveAspect = true;

            _displayDescription = displayDescription;
            _buyItem = buyItem;

            gameObject.SetActive(true);

            _buyButon.SetClick(onBuyClick);
            _buyButon.SetText(ItemConfig.Price.ToString());
        }


        private void onBuyClick() => _buyItem?.Invoke();

        public void OnPointerClick(PointerEventData eventData)
        {
            _displayDescription(ItemConfig);
        }
    }
}
