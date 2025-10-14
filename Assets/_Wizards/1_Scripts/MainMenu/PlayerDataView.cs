using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class PlayerDataView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _playerDisplay;
        [SerializeField] private StatDisplayView _coins;
        [SerializeField] private StatDisplayView _energy;
        [SerializeField] private StatDisplayView _health;
        [SerializeField] private StatDisplayView _speed;
        [SerializeField] private StatDisplayView _damage;
        [SerializeField] private StatDisplayView _defence;
        [SerializeField] private RegisterView _registerView;


        public void Display(IMenuInfo gameData)
        {
            _nameText.text = gameData.PlayerModel.Name;

            _coins.SetValue(gameData.PlayerModel.Bonuses[BonusType.Coin]);
            _energy.SetValue(gameData.PlayerModel.Bonuses[BonusType.Souls]);
            _health.SetValue(gameData.PlayerModel.Stats.Health);
            _speed.SetValue(gameData.PlayerModel.Stats.Damage);
            _damage.SetValue(gameData.PlayerModel.Stats.Speed);
            _defence.SetValue(gameData.PlayerModel.Stats.Defence);
        }

        public void UpdateDisplay(Sprite playerDisplay)
        {
            _playerDisplay.sprite = playerDisplay;
            _playerDisplay.preserveAspect= true;
        }

        private int CalculateScore(List<Location> locations)
        {
            int res = 0;
            foreach (Location location in locations) res += location.Score;
            return Mathf.RoundToInt(res / 4f);
        }

        public void Register(Action<string> onRegister)
        {
            Debug.Log("Registering new player");
            _registerView.ShowRegisterMenu(onRegister);
        }
    }
}
