using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class StatDisplayView : MonoBehaviour
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private Image _iconPlace;
        [SerializeField] private TextMeshProUGUI _valueText;
        
        void Start()
        {
            _iconPlace.sprite = _icon;
        }

        public void SetValue(int value) => _valueText.text = $"{value}";
    }
}
