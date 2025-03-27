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

        public void Init(List<Location> locations, Action<LocationType> onStart)
        {
            for (int i = 0; i < Mathf.Min(locations.Count, _locationDisplays.Count); i++)
            {
                LocationType t = locations[i].Type;
                _locationDisplays[i].Display(locations[i].Sprite, () => onStart?.Invoke(t));
            }
        }
    }
}