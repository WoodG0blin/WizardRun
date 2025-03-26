using System;
using UnityEngine;
using TMPro;

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
            _scoreText.text = $"{gameData.Score}";
            _coinsText.text = $"{gameData.Bonuses}";
        }

        public void Register(Action<string> onRegister)
        {
            Debug.Log("Registering new player");
            _registerView.ShowRegisterMenu(onRegister);
        }
    }
}
