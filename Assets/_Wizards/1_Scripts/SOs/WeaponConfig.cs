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
    public class WeaponConfig : ItemConfig
    {
        [field: Header("ACTION CONFIGS")]
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int ActionDistance { get; private set; }
        [field: SerializeField] public int FireForce { get; private set; }
        [field: SerializeField] public GameObject Ammo { get; private set; }
    }
}
