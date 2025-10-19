using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WizardsPlatformer
{
    internal class LevelDisplayView : MonoBehaviour
    {
        [SerializeField] private Image _playerImage;
        [SerializeField] private Image _bossImage;
        [SerializeField] private TextMeshProUGUI _playerNameText;

        [SerializeField] private Slider _playerHealth;
        [SerializeField] private Slider _bossHealth;
        [SerializeField] private StatDisplayView _coins;
        [SerializeField] private StatDisplayView _souls;
        [SerializeField] private StatDisplayView _artifacts;

        [SerializeField] private Slider _levelClearance;

        public void InitPlayer(PlayerModel player)
        {
            _playerNameText.text = player.Name;
            _playerHealth.maxValue = player.Stats.MaxHealth;
            _playerHealth.value = player.Stats.Health;

            SetBonusCount(BonusType.Coin, player.Bonuses[BonusType.Coin]);
            SetBonusCount(BonusType.Souls, player.Bonuses[BonusType.Souls]);
            SetBonusCount(BonusType.Artifacts, player.Bonuses[BonusType.Artifacts]);
        }

        public void SetHealth(int health) => _playerHealth.value = health;
        public void SetBonusCount(BonusType type, int value)
        {
            var display = type switch
            {
                BonusType.Coin => _coins,
                BonusType.Souls => _souls,
                BonusType.Artifacts => _artifacts
            };
            display.SetValue(value);
        }
        public void SetLevelClearanceValue(float value) => _levelClearance.value = 1f - value ;
    }
}