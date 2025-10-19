using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class ControlAllocationView : MenuPanelView
    {
        [SerializeField] private ButtonView _mainAttackButton;
        [SerializeField] private ButtonView _extra1Button;
        [SerializeField] private ButtonView _extra2Button;
        [SerializeField] private ButtonView _extra3Button;
        [SerializeField] private ButtonView _extra4Button;

        [Space(10)]
        [SerializeField] ButtonView _artButtonPrefab;
        [SerializeField] Transform _artButtonsContainer;
        [SerializeField] ButtonView _finishButton;

        private List<ButtonView> _controlButtons;

        private ItemConfig _currentItem;
        private List<ItemConfig> _setupControls;

        private List<ItemConfig> _changedItems;

        protected override void OnInit()
        {
            _controlButtons = new()
            {
                _mainAttackButton,
                _extra1Button,
                _extra2Button,
                _extra3Button,
                _extra4Button
            };

            for (int i = 1; i < _controlButtons.Count; i++)
            {
                int index = i;
                _controlButtons[i].SetClick(() => OnControlSelected(index));
                _controlButtons[i].SetActive(false);
            }
        }

        protected override void OnActivation()
        {
            var expl = menuInfo.PlayerModel.EquippedArtifacts.Where(a => a.Config.HasExplicitProperty).ToList();

            _setupControls = new();
            foreach(var b in _controlButtons) _setupControls.Add(null);
            _changedItems = new();

            foreach (var ex in expl)
            {
                ButtonView next = GameObject.Instantiate(_artButtonPrefab.gameObject, _artButtonsContainer).GetComponent<ButtonView>();
                next.SetImage(ex.Icon);
                int control = ex.Config.ControlIndex;
                if(_setupControls[control] == null)
                {
                    _setupControls[control] = ex.Config;
                    _controlButtons[control].SetImage(ex.Icon);
                }
                next.SetClick(() => OnArtifactSelect(ex.Config));
            }
            for (int i = 0; i < _controlButtons.Count; i++)
                _controlButtons[i].SetActive(false);

        }

        public void Display(Action<List<ItemConfig>> onUpdateAllocations, ItemConfig newAllocation = null)
        {
            SetActive(true);

            _finishButton.SetClick(() => onUpdateAllocations(_changedItems));

            if (newAllocation != null)
            {
                ButtonView next = GameObject.Instantiate(_artButtonPrefab.gameObject, _artButtonsContainer).GetComponent<ButtonView>();
                next.SetImage(newAllocation.Icon);
                next.SetClick(() => OnArtifactSelect(newAllocation));
                OnArtifactSelect(newAllocation);
            }

        }

        private void OnArtifactSelect(ItemConfig selected)
        {
            foreach(var b in _controlButtons) b.SetActive(true);
            _currentItem = selected;
        }

        private void OnControlSelected(int index)
        {
            if (_setupControls[index] != null)
            {
                _setupControls[index].ControlIndex = -1;
                if(!_changedItems.Contains(_setupControls[index])) _changedItems.Add(_setupControls[index]);

            }

            //clear previous
            int lastIndex = _currentItem.ControlIndex;
            if(lastIndex >= 0)
            {
                _setupControls[lastIndex] = null;
                _controlButtons[lastIndex].SetImage(null);
            }

            //set new
            _currentItem.ControlIndex = index;
            _setupControls[index] = _currentItem;
            _controlButtons[index].SetImage(_currentItem.Icon);

            if(!_changedItems.Contains(_currentItem)) _changedItems.Add(_currentItem);
        }
    }
}
