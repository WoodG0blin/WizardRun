using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IWeapon
    {
        bool WeaponReady { get; }
        void Fire();
        void SetDirection(Vector3 direction);
    }
}
