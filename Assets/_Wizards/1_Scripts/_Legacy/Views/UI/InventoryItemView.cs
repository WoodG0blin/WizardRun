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

        private bool _isSelected;
        private Action<bool> _onClick;

        public void Init(ItemConfig item, Action<bool> onSelection)
        {
            gameObject.SetActive(true);
            _icon.sprite = item.Icon;
            _onClick = onSelection;
            _button.onClick.AddListener(Select);
            _isSelected = false;
        }

        private void Select()
        {
            _isSelected = !_isSelected;

            _background.color = _isSelected ? _selected : _unselected;

            _onClick?.Invoke(_isSelected);
        }
    }
}
