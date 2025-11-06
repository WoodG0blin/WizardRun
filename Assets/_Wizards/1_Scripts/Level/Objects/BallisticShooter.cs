using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardsPlatformer
{
    internal class BallisticShooter : ActiveObject
    {
        private new BallisticShooterView view;

        public BallisticShooter(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition) { }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<BallisticShooterView>();

        protected override void OnInitiateView()
        {
            view = base.view as BallisticShooterView;

            view.SetUpdateActions(SetAttack);

            Barrel = view.Barrel;

            base.OnInitiateView();
        }
    }
}
