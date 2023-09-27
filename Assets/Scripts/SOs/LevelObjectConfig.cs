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
        bool HasStats { get; }
        float MaxHealth { get; }
        float Speed { get; }
        bool HasWeapon { get; }
        IWeaponConfig WeaponConfig { get; }
        bool IsKillable { get; }
        int BonusesOnKill { get; }
    }

    [CreateAssetMenu(fileName = nameof(LevelObjectConfig), menuName = "Configs/" + nameof(LevelObjectConfig), order = 5)]
    internal class LevelObjectConfig : ScriptableObject, ILevelObjectConfig
    {
        [SerializeField] protected string _name;
        [SerializeField] protected GameObject _prefab;
        [SerializeField] protected AnimationSequence[] _animations;

        [Space(10)]
        [SerializeField] protected float _maxHealth;
        [SerializeField] protected float _speed;

        [Space(10)]
        [SerializeField] protected WeaponConfig _weaponConfig;

        [Space(10)]
        [SerializeField] protected int _bonusesOnKill;


        public string Name { get => _name; }
        public GameObject  Prefab{ get => _prefab; }
        public AnimationSequence[] Animations { get => _animations; }
        public bool HasStats { get => MaxHealth > 0; }
        public float MaxHealth { get => _maxHealth; }
        public float Speed { get => _speed; }
        public bool HasWeapon { get => _weaponConfig != null; }
        public IWeaponConfig WeaponConfig { get => _weaponConfig; }
        public bool IsKillable { get => _bonusesOnKill > 0; }
        public int BonusesOnKill { get => _bonusesOnKill; }


        public IReadOnlyList<LevelObjectConfig> Configs => new List<LevelObjectConfig>() { this};
    }
}