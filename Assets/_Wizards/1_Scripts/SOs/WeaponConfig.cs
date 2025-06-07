using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = "New" + nameof(WeaponConfig), menuName = "Configs/" + nameof(WeaponConfig))]
    public class WeaponConfig : ItemConfig
    {
        [field: Header("WEAPON CONFIGS")]
        [field: SerializeField] public bool IsRanged { get; protected set; }
        [field: SerializeField] public bool IsBallistic { get; protected set; }
        [field: SerializeField] public float FireForce { get; protected set; }
        [field: SerializeField] public GameObject Ammo { get; protected set; }

    }
}