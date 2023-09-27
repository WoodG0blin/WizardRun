using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal interface IInventoryView: IView
    {
        UnityAction OnApply { set; }
        UnityAction OnReturn { set; }

        void Clear();
        void Display(IEnumerable<IItem> items, Action<string> OnItemClick);
        void Select(string itemID, bool selection);
    }

    internal class InventoryView : View, IInventoryView
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _applyButton;

        public UnityAction OnReturn { set => _backButton.onClick.AddListener(value); }
        public UnityAction OnApply { set => _applyButton.onClick.AddListener(value); }

        private Dictionary<string, ItemView> _itemViews = new();
        public void Display(IEnumerable<IItem> items, Action<string> OnItemClick)
        {
            foreach (IItem item in items)
                _itemViews[item.ID] = DisplayItem(item, OnItemClick);
        }

        public void Clear()
        {
            foreach (ItemView item in _itemViews.Values)
            {
                item.DeInit();
                GameObject.Destroy(item.gameObject);
            }
            _itemViews.Clear();
        }

        public void Select(string itemID, bool selection) => _itemViews[itemID]?.Select(selection);

        private ItemView DisplayItem(IItem item, Action<string> onItemClick)
        {
            ItemView temp = GameObject.Instantiate(_itemPrefab, _container).GetComponent<ItemView>();
            temp.Init(item, () => { onItemClick.Invoke(item.ID); });
            return temp;
        }

        protected override void OnDestruction() => Clear();
    }
}
