using System.Collections.Generic;
using System;

namespace WizardsPlatformer
{
    internal interface IUpgradable
    {
        Dictionary <ActivatorType, IUpgrade> Upgrades { get; }
        CharacterStats Stats { get; }
        IJump Jumper { get; }
        IWeapon Weapon { get; }

        void Reset();
    }
}
