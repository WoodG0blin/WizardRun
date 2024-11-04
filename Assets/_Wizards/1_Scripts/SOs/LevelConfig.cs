using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(LevelConfig), menuName = "Configs/" + nameof(LevelConfig), order = 11)]
    internal sealed class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public LevelObjectConfig Player { get; private set; }
        [field: SerializeField] public GroundsConfig Grounds { get; private set; }
        [field: SerializeField] public InputConfig Inputs { get; private set; }
        [field: SerializeField] public MenuConfig LevelDisplay { get; private set; }
    }
}