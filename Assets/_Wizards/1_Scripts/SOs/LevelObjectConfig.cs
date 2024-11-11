using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface ILevelObjectConfig: IDataSource<LevelObjectConfig>
    {
        string Name { get; }
        AnimationSequence[] Animations { get; }
        GameObject Prefab { get; }
        int MaxHealth { get; }
        int Speed { get; }
        ItemConfig WeaponConfig { get; }
        int BonusesOnKill { get; }
    }

    [CreateAssetMenu(fileName = nameof(LevelObjectConfig), menuName = "Configs/" + nameof(LevelObjectConfig), order = 5)]
    internal class LevelObjectConfig : ScriptableObject, ILevelObjectConfig
    {
        [SerializeField] private string _name;
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public AnimationSequence[] Animations { get; private set; }

        [field: Space(10)]
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public int JumpForce { get; private set; }

        [Space(10)]
        [SerializeField] private ItemConfig _weaponConfig;

        [field: Space(10)]
        [field: SerializeField] public int DifficultyLevel { get; private set; }
        [field: SerializeField] public int BonusesOnKill { get; private set; }


        public string Name { get => _name; }
        public ItemConfig WeaponConfig => _weaponConfig;

        public IReadOnlyList<LevelObjectConfig> Configs => new List<LevelObjectConfig>() { this};
    }
}