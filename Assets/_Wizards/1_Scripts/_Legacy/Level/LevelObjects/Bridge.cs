using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Bridge : LevelObject
    {
        private float _angle;
        public Bridge(Vector2 position, float angle) : base("Bridge", position) { _angle = angle; }

        protected override void OnInitiateView(GameObject gameObject) =>
            gameObject.AddComponent<BridgeView>().Init(_angle);
    }
}
