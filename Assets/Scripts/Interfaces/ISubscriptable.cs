using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WizardsPlatformer
{
    public interface ISubscriptable<T>
    {
        public T Value { get; }
        public void SubscribeOnValueChange(Action<T> onValueChange);
        public void UnsubscribeOnValueChange(Action<T> onValueChange);
    }
}
