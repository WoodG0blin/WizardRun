using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class ItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;

        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private GameObject _unSelectedBackground;

        private bool _isSelected;
        private Action<bool> _onClick;

        public void Init(ItemConfig item, Action<bool> onSelection)
        {
            gameObject.SetActive(true);
            _title.text = item.Name;
            _icon.sprite = item.Icon;
            _onClick = onSelection;
            _button.onClick.AddListener(Select);
            _isSelected = false;
        }

        private void Select()
        {
            _isSelected = !_isSelected;

            _selectedBackground.SetActive(_isSelected);
            _unSelectedBackground.SetActive(!_isSelected);

            _onClick?.Invoke(_isSelected);
        }
    }
}
