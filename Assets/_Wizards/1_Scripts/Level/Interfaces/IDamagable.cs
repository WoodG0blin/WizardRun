using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IDamagable
    {
        void ReceiveDamage(int damage);
    }
}