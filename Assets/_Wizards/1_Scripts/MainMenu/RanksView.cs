using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class RanksView : MenuPanelView
    {
        [SerializeField] private GameObject _ranksWindow;
        [SerializeField] private Button _ranksButton;
        [SerializeField] private GameObject _statsWindow;
        [SerializeField] private Button _statsButton;

        protected override void OnInit()
        {
            _ranksButton.onClick.RemoveAllListeners();
            _statsButton.onClick.RemoveAllListeners();
            _ranksButton.onClick.AddListener(() => ShowStats(false));
            _statsButton.onClick.AddListener(() => ShowStats(true));
        }

        private void ShowStats(bool stats)
        {
            _statsWindow.SetActive(stats);
            _ranksWindow.SetActive(!stats);
        }
    }
}
