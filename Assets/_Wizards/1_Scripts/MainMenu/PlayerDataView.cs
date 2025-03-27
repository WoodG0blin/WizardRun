using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace WizardsPlatformer
{
    public class PlayerDataView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private RegisterView _registerView;


        public void Display(IMenuInfo gameData)
        {
            _nameText.text = gameData.PlayerModel.Name;
            _scoreText.text = $"{CalculateScore(gameData.Locations)}";
            _coinsText.text = $"{gameData.PlayerModel.Bonuses}";
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
