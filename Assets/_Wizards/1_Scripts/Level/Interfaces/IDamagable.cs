using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IDamagable
    {
        Action<int> OnReceiveDamage { get; set; }
        void ReceiveDamage(int damage);
    }
}