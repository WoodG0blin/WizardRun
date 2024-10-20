using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class RewardsView : View
    {
        private const string LAST_DAILY = "LastDailyReward";
        private const string COUNT_WEEKLY = "CountConsequtiveInWeek";
        private const string COUNT_MONTHLY = "CountConsequtiveInMonth";
        private const double DAY_LENGTH_MINUTES = 2d;

        [SerializeField] private Button _daily;
        [SerializeField] private Button _weekly;
        [SerializeField] private Button _monthly;

        private DateTime _now;
        private DateTime _lastDaily;
        private int _countWeekly;
        private int _countMonthly;

        public event Action<BonusType, int> OnRewardCollect;

        public void OnDailyClick()
        {
            OnRewardCollect?.Invoke(BonusType.coin, 10);

            string now = _now.ToString();
            //Debug.Log($"Writing current dateTime {now}");
            PlayerPrefs.SetString(LAST_DAILY, now);

            if (_now.Subtract(_lastDaily).TotalSeconds < 2 * DAY_LENGTH_MINUTES)
            {
                _countWeekly++;
                _countMonthly++;
            }
            else
            {
                _countWeekly = 1;
                _countMonthly = 1;
            }

            PlayerPrefs.SetInt(COUNT_WEEKLY, _countWeekly);
            PlayerPrefs.SetInt(COUNT_MONTHLY, _countMonthly);
        }

        public void OnWeeklyClick()
        {
            OnRewardCollect?.Invoke(BonusType.coin, 50);

            _countWeekly = 0;
            PlayerPrefs.SetInt(COUNT_WEEKLY, _countWeekly);
        }

        public void OnMonthlyClick()
        {
            OnRewardCollect?.Invoke(BonusType.coin, 200);

            _countMonthly = 0;
            PlayerPrefs.SetInt(COUNT_MONTHLY,_countMonthly);
        }

        private void FixedUpdate()
        {
            GetDates();

            SetActiveDaily();
            SetActiveWeekly();
            SetActiveMonthly();
        }


        private void GetDates()
        {
            _now = DateTime.Now;
            string lastDaily = PlayerPrefs.GetString(LAST_DAILY);
            if (lastDaily != "") _lastDaily = DateTime.Parse(lastDaily);
            else _lastDaily = DateTime.MinValue;

            _countWeekly = PlayerPrefs.GetInt(COUNT_WEEKLY);
            _countMonthly = PlayerPrefs.GetInt(COUNT_MONTHLY);

            //Debug.Log($"Date difference is {_now - _lastDaily}");
        }

        private void SetActiveDaily()
        {
            _daily.interactable = _now.Subtract(_lastDaily).TotalSeconds > DAY_LENGTH_MINUTES;
        }

        private void SetActiveWeekly()
        {
            _weekly.interactable = _countWeekly >= 7d;
        }

        private void SetActiveMonthly()
        {
            _monthly.interactable = _countMonthly >= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        }
    }
}
