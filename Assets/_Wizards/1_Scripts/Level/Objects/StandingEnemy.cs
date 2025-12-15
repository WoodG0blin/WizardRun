using UnityEngine;

namespace WizardsPlatformer
{
    internal class StandingEnemy : ActiveObject
    {
        private new EnemyView view;

        public StandingEnemy(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition) { }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<EnemyView>();

        protected override void OnInitiateView()
        {
            view = base.view as EnemyView;
            Barrel = view.Barrel;
            base.OnInitiateView();
        }

        protected override void ActionsOnUpdate()
        {
            base.ActionsOnUpdate();
            view.SetLookDirection(currentPlayerPosition);

            if (Weapon != null && Weapon.CheckAction(TargetDirection * view.XDirection))
                ExecuteAttackAction();

        }
    }
}
