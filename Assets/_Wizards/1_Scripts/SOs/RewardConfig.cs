using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = "New" + nameof(RewardConfig), menuName = "Configs/" + nameof(RewardConfig))]
    public class RewardConfig : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public Sprite DisplayImage { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public List<ItemConfig> Items { get; private set; }
        [field: SerializeField] public List<Bonus> Bonuses { get; private set; }
        [field: SerializeField] public bool IsAccountable { get; private set; } = true;
        [field: SerializeField] public int ConditionValue { get; private set; }
    }

    public class Reward
    {
        public class RewardExecutor
        {
            protected string ID;
            public RewardExecutor(string ID) { this.ID = ID; }
            public virtual bool CheckCondition(int conditionValue, IPlayerModel player, RewardData reward)
            {
                return true;
            }

            public virtual void Execute(IPlayerModel player, Action onSuccess)
            {
                onSuccess?.Invoke();
            }

            public virtual string GetRewardDataInfo() => "";
        }


        private RewardConfig _config;
        private RewardExecutor _executor;
        private Action _onApplied;

        public Sprite Icon => _config.DisplayImage;
        public string Description => _config.Description;
        public List<ItemConfig> Items =>_config.Items;
        public List<Bonus> Bonuses => _config.Bonuses;


        public Reward(RewardConfig config)
        {
            _config = config;
            _executor = _config.ID switch
            {
                string a when a.Contains("Adds") => new AddsReward(_config.ID),
                string a when a.Contains("Daily") => new DailyReward(_config.ID),
                string a when a.Contains("GameLast") => new GameLastReward(_config.ID),
                _ => new RewardExecutor(_config.ID)
            };
        }

        public bool IsConditionMet(IPlayerModel player)
        {
            if (!_config.IsAccountable) return true;

            var data = player.CollectedRewards.Where(r => r.ID == _config.ID).FirstOrDefault();
            return _executor.CheckCondition(_config.ConditionValue, player, data);
        }


        public void Apply(IPlayerModel player, Action onApplied)
        {
            _onApplied = onApplied;

            if(IsConditionMet(player))
            {
                _executor.Execute(player, () => onExecuted(player));
            }
            else _onApplied?.Invoke();
        }

        private void onExecuted(IPlayerModel player)
        {
            foreach (var itemConfig in _config.Items)
            {
                player.AddArtifact(itemConfig);
            }
            foreach (var bonus in _config.Bonuses)
            {
                player.AddBonus(bonus.Type, bonus.Value);
            }
            if (_config.IsAccountable)
                player.AccountReward(new(_config.ID, _executor.GetRewardDataInfo()));
            _onApplied?.Invoke();
        }
    }

    public class AddsReward : Reward.RewardExecutor
    {
        public AddsReward(string ID) : base(ID) { } 
        public override void Execute(IPlayerModel player, Action onSuccess)
        {
            Debug.Log("Showing ads for reward");
            onSuccess?.Invoke();
        }
    }

    public class DailyReward : Reward.RewardExecutor
    {
        private int daysPlayed = 0;

        public DailyReward(string ID) : base(ID) { }

        public override bool CheckCondition(int conditionValue, IPlayerModel player, RewardData reward)
        {
            if (reward == null)
            {
                player.AccountReward(new(ID, GetRewardDataInfo()));
                return conditionValue <= 1;
            }

            bool res = false;

            string count = reward.data.Substring(0, reward.data.IndexOf('+'));
            string date = reward.data.Substring(reward.data.IndexOf('+') + 1);

            daysPlayed = int.Parse(count);
            DateTime lastAccounted = DateTime.Parse(date);

            if (lastAccounted.Day != DateTime.Now.Day && (int)(DateTime.Now - player.LastEntryDate).TotalDays == 1)
            {
                daysPlayed++;
                res = daysPlayed >= conditionValue;
                if(res) daysPlayed = 0;
            }

            player.AccountReward(new(ID, GetRewardDataInfo()));

            return res;
        }

        public override string GetRewardDataInfo() => daysPlayed.ToString() + "+" + DateTime.Now.ToString();
    }

    public class GameLastReward : Reward.RewardExecutor
    {
        public GameLastReward(string ID) : base(ID) { }
        public override bool CheckCondition(int conditionValue, IPlayerModel player, RewardData reward)
        {
            if(reward == null)
                player.AccountReward(new(ID, GetRewardDataInfo()));

            DateTime lastEntered = player.CurrentEntryDate;
            DateTime lastClaimed = reward == null ? DateTime.Now : DateTime.Parse(reward.data);

            DateTime counter = lastEntered > lastClaimed ? lastEntered : lastClaimed;


            int elapsedMinutes = (int)(DateTime.Now - counter).TotalMinutes;
            return elapsedMinutes >= conditionValue;
        }
        public override string GetRewardDataInfo() => DateTime.Now.ToString();
    }
}