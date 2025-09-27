using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = nameof(LevelObjectConfig), menuName = "Configs/" + nameof(LevelObjectConfig), order = 5)]
    internal class LevelObjectConfig : ScriptableObject
    {
        [SerializeField] private string nameTag;
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [field: Space(10), Header("STATS")]
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public int JumpForce { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }

        [field: Space(10), Header("BASE WEAPON")]
        [field: SerializeField] public ArtifactPropertyConfig WeaponConfig { get; protected set; }


        [field: Space(10), Header("LEVEL CONFIGS")]
        [field: SerializeField] public int DifficultyLevel { get; private set; }
        [field: SerializeField] public int BonusesOnKill { get; private set; }


        public string Name => nameTag;
    }
}