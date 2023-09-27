using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal enum UpgradeType { SpeedIncrease, IncreasedJump, BasicAttack, BasicDefence, MultipleJump}
    internal enum ActivatorType { OnStats, OnJump, OnAttack, OnDefence}

    [Serializable]
    internal class UpgradeConfig
    {
        [field: SerializeField] public ActivatorType Activator { get; private set; }
        [field: SerializeField] public UpgradeType Upgrade { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }

    internal interface IUpgrade
    {
        UpgradeConfig Config { get; }

        void Activate();
        void Add(IUpgrade next);
    }

    internal abstract class Upgrade : IUpgrade
    {
        protected readonly UpgradeConfig _config;
        protected IUpgrade _next;
        protected Upgrade(UpgradeConfig config) => _config = config;

        public void Activate()
        {
            OnActivation();
            _next?.Activate();
        }

        protected abstract void OnActivation();

        public void Add(IUpgrade next)
        {
            if (_next != null)
                _next.Add(next);
            else
                _next = next;
        }

        public Action<bool> OnFinish;
        public UpgradeConfig Config { get => _config; }
    }
}
