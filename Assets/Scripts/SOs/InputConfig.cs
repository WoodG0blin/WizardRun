using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(InputConfig), menuName = "Configs/" + nameof(InputConfig), order = 9)]
    internal sealed class InputConfig : ScriptableObject
    {
#if MOBILE_INPUT
        private bool mobileInput = true;
#else
        private bool mobileInput = false; 
#endif
        [field: SerializeField] public GameObject MobileInputPrefab { get; private set; }
        [field: SerializeField] public GameObject KeyboardInputPrefab { get; private set; }

        public GameObject Prefab => mobileInput ? MobileInputPrefab: KeyboardInputPrefab;
    }
}