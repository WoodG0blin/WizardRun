using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IWeaponConfig
    {
        int AttackDistance { get; }
        int CoolDown { get; }
        int Damage { get; }
        int FireForce { get; }
        bool IsRanged { get; }
        string Name { get; }
        GameObject AmmoPrefab { get; }
    }

    [CreateAssetMenu(fileName = nameof(WeaponConfig), menuName = "Configs/" + nameof(WeaponConfig), order = 5)]
    internal class WeaponConfig : ScriptableObject, IWeaponConfig
    {
        [SerializeField] protected string _name;
        [SerializeField] protected GameObject _ammo;

        [Space(10)]
        [SerializeField] protected bool _isRanged;
        [SerializeField] protected int _damage;
        [SerializeField] protected int _attackDistance;
        [SerializeField] protected int _cooldown;
        [SerializeField] protected int _fireForce;

        public string Name { get => _name; }
        public GameObject AmmoPrefab { get => _ammo; }
        public bool IsRanged { get => _isRanged; }
        public int Damage { get => _damage; }
        public int AttackDistance { get => _attackDistance; }
        public int CoolDown { get => _cooldown; }
        public int FireForce { get => _fireForce; }
    }
}
