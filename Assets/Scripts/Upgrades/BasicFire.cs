using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class BasicFire : Upgrade
    {
        public BasicFire(UpgradeConfig config) : base(config) { }
        private IWeapon _weapon;

        public void Init(IWeapon weapon) => _weapon = weapon;
        protected override void OnActivation()
        {
            _weapon?.Fire();
        }
    }
}
