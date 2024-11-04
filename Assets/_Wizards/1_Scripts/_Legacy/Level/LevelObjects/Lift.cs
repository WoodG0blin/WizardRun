using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Lift : JumpPlatform
    {
        private int _deltaHeight;

        public Lift(Vector2 position, int deltaHeight) : base(position)
        {
            _deltaHeight = deltaHeight;
        }

        protected override void OnInitiateView(GameObject gameObject) =>
            gameObject.AddComponent<LiftView>().Init(_deltaHeight);
    }
}
