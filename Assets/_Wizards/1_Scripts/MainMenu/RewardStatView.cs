using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class RewardStatView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMPro.TextMeshProUGUI _valueText;

        public void Display(Sprite icon, string text)
        {
            _icon.sprite = icon;
            _valueText.text = text;
        }

    }
}
