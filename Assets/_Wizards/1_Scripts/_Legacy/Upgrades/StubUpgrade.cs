using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class StubUpgrade : Upgrade
    {
        public static StubUpgrade Default = new StubUpgrade();

        public StubUpgrade() : base(null) {}

        protected override void OnActivation()
        {
        }
    }
}