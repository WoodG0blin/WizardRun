using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface ILevelObjectConfig: IDataSource<LevelObjectConfig>
    {
        string Name { get; }
        GameObject Prefab { get; }
        int MaxHealth { get; }
        int Speed { get; }
        WeaponConfig WeaponConfig { get; }
        int BonusesOnKill { get; }
    }

    [CreateAssetMenu(fileName = nameof(LevelObjectConfig), menuName = "Configs/" + nameof(LevelObjectConfig), order = 5)]
    internal class LevelObjectConfig : ScriptableObject, ILevelObjectConfig
    {
        [SerializeField] private string _name;
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [field: Space(10), Header("STATS")]
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public int JumpForce { get; private set; }

        [Space(10), Header("BASE WEAPON")]
        [SerializeField] private WeaponConfig _weaponConfig;
        [field: SerializeField] public int Cooldown { get; protected set; }
        [field: SerializeField] public int Damage { get; protected set; }
        [field: SerializeField] public float ActionDistance { get; protected set; }
        [field: SerializeField] public bool IsRanged { get; protected set; }
        [field: SerializeField] public float FireForce { get; protected set; }
        [field: SerializeField] public GameObject Ammo { get; protected set; }


        [field: Space(10), Header("LEVEL CONFIGS")]
        [field: SerializeField] public int DifficultyLevel { get; private set; }
        [field: SerializeField] public int BonusesOnKill { get; private set; }


        public string Name { get => _name; }
        public WeaponConfig WeaponConfig => _weaponConfig;

        public IReadOnlyList<LevelObjectConfig> Configs => new List<LevelObjectConfig>() { this};
    }
}