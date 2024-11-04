using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IJumpUpgrade : IUpgrade
    {
        void Init(IJump jumper);
    }

    internal class BasicJump : Upgrade, IJumpUpgrade
    {
        protected IJump _jumper;
        public BasicJump(UpgradeConfig config) : base(config) {}

        public void Init(IJump jumper) => _jumper = jumper;
        protected override void OnActivation()
        {
            if (_jumper.AccessContacts().HasContactDown)
            {
                _jumper.Jump(_config.Value);
                OnFinish?.Invoke(true);
            }
        }
    }
}