using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IDamagable
    {
        event Action<int> OnReceiveDamage;
        void ReceiveDamage(int damage);
    }
}