using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Bridge : SimpleObject
    {
        private float _angle;
        public Bridge(LevelObjectConfig config, Vector2 position, float angle)
            : base(config, position)
            => _angle = angle;

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<BridgeView>();

        protected override void OnInitiateView() =>
            (view as BridgeView).Init(_angle);
    }
}
