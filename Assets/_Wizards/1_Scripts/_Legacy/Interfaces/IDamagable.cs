using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IDamagable
    {
        event Action<float> OnReceiveDamage;
        void ReceiveDamage(float damage);
    }
}