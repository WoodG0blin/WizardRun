using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardsPlatformer
{
    internal class BallisticShooter : StandingEnemy
    {
        private new EnemyView view;

        public BallisticShooter(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition) { }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<BallisticShooterView>();

        protected override void OnInitiateView()
        {
            base.OnInitiateView();
            view = base.view as EnemyView;
        }
    }
}
