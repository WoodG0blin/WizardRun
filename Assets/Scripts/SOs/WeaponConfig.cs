using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IWeaponConfig
    {
        float AttackDistance { get; }
        float CoolDown { get; }
        float Damage { get; }
        float FireForce { get; }
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
        [SerializeField] protected float _damage;
        [SerializeField] protected float _attackDistance;
        [SerializeField] protected float _cooldown;
        [SerializeField] protected float _fireForce;

        public string Name { get => _name; }
        public GameObject AmmoPrefab { get => _ammo; }
        public bool IsRanged { get => _isRanged; }
        public float Damage { get => _damage; }
        public float AttackDistance { get => _attackDistance; }
        public float CoolDown { get => _cooldown; }
        public float FireForce { get => _fireForce; }
    }
}
