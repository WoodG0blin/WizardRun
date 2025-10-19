using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal class InventoryDisplayView : MenuPanelView
    {
        [SerializeField] private MenuPanelView _equip;
        [SerializeField] private MenuPanelView _craft;
        [SerializeField] private InventoryView _inventory;

        private MenuPanelsManager _subPanelsManager;

        protected override void OnInit()
        {
            _inventory.Container = transform;

            _subPanelsManager = new(
                panels: new() { _equip, _craft },
                input: menuInfo);

            _inventory.Init(menuInfo);

            _subPanelsManager.ActivatePanel(_equip);
        }
    }
}
