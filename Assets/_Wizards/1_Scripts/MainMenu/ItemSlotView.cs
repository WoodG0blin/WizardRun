using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class ItemSlotView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMPro.TextMeshProUGUI _descriptionText;
        [SerializeField] private RewardStatView _rewardStatView;
        [SerializeField] private ButtonView _applyButton;

        [Space(10)]
        [SerializeField] private Sprite _coinSprite;
        [SerializeField] private Sprite _soulSprite;
        [SerializeField] private Sprite _artifactSprite;


        public void Init(ShopItemConfig item, Action apply)
        {
            SetMainControls(
                icon: item.Item == null ? item.DisplayImage : item.Item.Icon,
                description: item.Item != null ? item.Item.Name : $"{item.Bonus.Type} +{item.Bonus.Value}",
                apply);

            List<(Sprite icon, string value)> displayStats = new();
            displayStats.Add((_coinSprite, item.Price.ToString()));

            SetStatsDisplay(displayStats);
        }
        public void Init(Reward item, Action apply)
        {
            SetMainControls(
                icon: item.Icon,
                description: item.Description,
                apply);

            List<(Sprite icon, string value)> displayStats = new();

            foreach (var i in item.Items) displayStats.Add((i.Icon, "1"));
            foreach (var b in item.Bonuses) displayStats.Add((setBonusImage(b.Type), b.Value.ToString()));

            SetStatsDisplay(displayStats);
        }

        private void SetMainControls(Sprite icon, string description, Action apply)
        {
            _icon.sprite = icon;
            _icon.preserveAspect = true;
            _descriptionText.text = description;
            if (apply == null)
                _applyButton.SetActive(false);
            else
                _applyButton.SetClick(apply);
        }

        private void SetStatsDisplay(List<(Sprite icon, string value)> displayStats)
        {
            Transform container = _rewardStatView.transform.parent;
            RewardStatView prefab = _rewardStatView;

            for (int i = container.childCount - 1; i > 0; i--)
            {
                GameObject.Destroy(container.GetChild(i).gameObject);
            }

            if (displayStats.Count > 0)
            {
                _rewardStatView.Display(displayStats[0].icon, displayStats[0].value);
                for (int i = 1; i < displayStats.Count; i++)
                {
                    var b = displayStats[i];
                    var view = GameObject.Instantiate(prefab, container);
                    view.Display(b.icon, b.value);
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());
            }
        }

        private Sprite setBonusImage(BonusType type) => type switch
        {
            BonusType.Coin => _coinSprite,
            BonusType.Souls => _soulSprite,
            BonusType.Artifacts => _artifactSprite
        };

    }
}
