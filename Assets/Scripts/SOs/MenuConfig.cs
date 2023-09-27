using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(MenuConfig), menuName = "Configs/" + nameof(MenuConfig), order = 10)]
    internal sealed class MenuConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}