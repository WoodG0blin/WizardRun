using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    public class WorldPanelView : MonoBehaviour
    {
        [SerializeField] private List<LocationDisplayView> _locationDisplays;

        public void Init(List<Location> locations, Action<Location> onStart)
        {
            for (int i = 0; i < Mathf.Min(locations.Count, _locationDisplays.Count); i++)
            {
                var loc = locations[i];
                _locationDisplays[i].Display(loc, (l) => onStart?.Invoke(l));
            }
        }

        public void UpdateValues()
        {
            foreach (var ld in _locationDisplays) ld.UpdateScore();
        }
    }
}