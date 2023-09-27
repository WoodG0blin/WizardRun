using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(GroundsConfig), menuName = "Configs/" + nameof(GroundsConfig), order = 8)]
    internal sealed class GroundsConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public AllLevelObjectsConfigs LevelObjectsConfigs { get; private set;}
    }
}