using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class RewardsView : MenuPanelView
    {
        [SerializeField] private List<RewardConfig> _rewardConfigs;
        [SerializeField] private Transform _rewardsContainer;
        [SerializeField] private RewardView _slotPrefab;

        public Action<BonusType, int> OnRewardCollect;

        private List<Reward> _rewards = new();

        protected override void OnInit()
        {
            foreach (var itemConfig in _rewardConfigs)
            {
                _rewards.Add(new Reward(itemConfig));
            }
        }

        protected override void OnActivation()
        {
            for(int i = _rewardsContainer.childCount -1; i >= 0; i--)
                GameObject.Destroy(_rewardsContainer.GetChild(i).gameObject);

            foreach (var item in _rewards)
            {
                var itemView = Instantiate(_slotPrefab, _rewardsContainer);
                itemView.Init(item, item.IsConditionMet(menuInfo.PlayerModel) ? () => Apply(item) : null);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rewardsContainer.GetComponent<RectTransform>());
        }

        private void Apply(Reward reward)
        {
            reward.Apply(menuInfo.PlayerModel, () => SetActive(true));
        }

    }
}
