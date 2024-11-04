using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal interface IItemView
    {
        void Init(IItem item, UnityAction onClickCallback);
        void Select(bool selection);
    }

    internal class ItemView : View, IItemView
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;

        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private GameObject _unSelectedBackground;

        public void Init(IItem item, UnityAction onClickCallback)
        {
            _title.text = item.Name;
            _icon.sprite = item.Icon;
            _button.onClick.AddListener(onClickCallback);
        }

        public void DeInit() => OnDisable();

        private void OnDisable()
        {
            _title.text = string.Empty;
            _icon.sprite = null;
            _button.onClick.RemoveAllListeners();
        }

        public void Select(bool selection)
        {
            _selectedBackground.SetActive(selection);
            _unSelectedBackground.SetActive(!selection);
        }
    }
}
