using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IKillable : IDamagable
    {
        event Action<BonusType, int> OnKilled;
    }
}
