using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public enum LevelObjectType
    {
        Obstacle = 0,
        Trap = 5,
        DirectShooter = 10,
        BallisticShooter = 15,
        MeleeEnemy = 20,
        Boss = 30
    }

    [CreateAssetMenu(fileName = nameof(LevelObjectConfig), menuName = "Configs/" + nameof(LevelObjectConfig), order = 5)]
    public class LevelObjectConfig : ScriptableObject
    {
        [SerializeField] private string nameTag;
        [field: SerializeField] public LevelObjectType Type { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [field: Space(10), Header("STATS")]
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int SpeedModifier { get; private set; }
        [field: SerializeField] public int Defence { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }

        [field: Space(10), Header("BASE WEAPON")]
        [field: SerializeField] public ArtifactPropertyConfig WeaponConfig { get; protected set; }
        [field: SerializeField] public AmmoConfig Ammo { get; protected set; }


        [field: Space(10), Header("LEVEL CONFIGS")]
        [field: SerializeField] public int DifficultyLevel { get; private set; }
        [field: SerializeField] public List<Bonus> BonusesOnKill { get; private set; } = new();

        public string Name => nameTag;
    }
}