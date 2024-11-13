using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(GroundsConfig), menuName = "Configs/" + nameof(GroundsConfig), order = 8)]
    internal sealed class GroundsConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite[] BackGroundSprites { get; private set; }
        [field: SerializeField] public Tile[] GroundTiles { get; private set; }
        [field: SerializeField] public GameObject Block { get; private set; }
        [field: SerializeField] public AllLevelObjectsConfigs ObjectsConfigs { get; private set; }
    }
}