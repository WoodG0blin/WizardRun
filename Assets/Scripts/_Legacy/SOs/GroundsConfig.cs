using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(GroundsConfig), menuName = "Configs/" + nameof(GroundsConfig), order = 8)]
    internal sealed class GroundsConfig : ScriptableObject
    {
        //[field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public Sprite[] BackGroundSprites { get; private set; }
        [field: SerializeField] public Tile[] GroundTiles { get; private set; }

        [SerializeField] private AllLevelObjectsConfigs LevelObjectsConfigs;

        public LevelObjectsRepository LevelObjectsRepository => new(LevelObjectsConfigs.Configs);
    }
}