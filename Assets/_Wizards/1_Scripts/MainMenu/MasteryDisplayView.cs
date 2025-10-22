using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class MasteryDisplayView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private TextMeshProUGUI _maxLevelText;
        [SerializeField] private TextMeshProUGUI _gradeText;
        [SerializeField] private Image _fillArea;

        public void SetValues(int current, int maxLevel, int grade)
        {
            _gradeText.text = grade.ToString();
            _maxLevelText.text = $"max: {maxLevel}";
            _currentLevelText.text = current.ToString();
            _fillArea.fillAmount = (float) current / maxLevel;
        }
    }
}
