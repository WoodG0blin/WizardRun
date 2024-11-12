using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class InventoryItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;

        [SerializeField] private Image _background;

        [SerializeField] private Color _selected;
        [SerializeField] private Color _unselected;


        public void Init(IArtifact item, Action onSelection)
        {
            gameObject.SetActive(true);
            _icon.sprite = item.Icon;
            _button.onClick.AddListener(() => onSelection?.Invoke());
        }

        public void Clear()
        {
            _icon.sprite = null;
            _button.onClick.RemoveAllListeners();
        }
    }
}
