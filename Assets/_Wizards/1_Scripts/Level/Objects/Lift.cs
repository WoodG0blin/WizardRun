using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Lift : SimpleObject
    {
        private int _deltaHeight;

        public Lift(LevelObjectConfig config, Vector2 position, int deltaHeight) : base(config, position)
        {
            _deltaHeight = deltaHeight;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<LiftView>();

        protected override void OnInitiateView() =>
            (view as LiftView).Init(_deltaHeight);
    }
}
