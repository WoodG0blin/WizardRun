using System.Collections.Generic;
using System;

namespace WizardsPlatformer
{
    internal interface IUpgradable
    {
        Dictionary <ActivatorType, IUpgrade> Upgrades { get; }
        Stats Stats { get; }
        IJump Jumper { get; }
        IWeapon Weapon { get; }

        void Reset();
    }
}
