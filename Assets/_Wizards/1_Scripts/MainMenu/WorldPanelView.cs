using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class WorldPanelView : MenuPanelView
    {
        [SerializeField] private List<LocationDisplayView> _locationDisplays;

        public Action<Location> OnStart;


        protected override void OnInit()
        {
            for (int i = 0; i < Mathf.Min(menuInfo.Locations.Count, _locationDisplays.Count); i++)
            {
                var loc = menuInfo.Locations[i];
                _locationDisplays[i].Display(loc, TriggerStart);
            }
        }

        protected override void OnActivation()
        {
            foreach (var ld in _locationDisplays) ld.UpdateScore();
        }

        private void TriggerStart(Location choice) => OnStart?.Invoke(choice);
    }
}