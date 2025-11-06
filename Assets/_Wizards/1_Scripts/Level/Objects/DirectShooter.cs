using UnityEngine;

namespace WizardsPlatformer
{
    internal class DirectShooter : ActiveObject
    {
        private new DirectShooterView view;
        private float _maxDistance;

        public DirectShooter(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            //weaponArtifact = new Weapon(config.WeaponConfig);
            //weapon = weaponArtifact.GetExecutor(ArtifactExecutorType.Attack);
            _maxDistance = 10;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<DirectShooterView>();

        protected override void OnInitiateView()
        {
            view = base.view as DirectShooterView;
            view.Init();
            view.SetUpdateActions(SetAttack);
            
            Barrel = view.Barrel;

            base.OnInitiateView();
        }

        protected override void SetAttack()
        {
            if (Weapon != null)
            {
                float xDir = view.RotateBarrelTowards(currentPlayerPosition).x;
                Direction = (currentPlayerPosition - view.Position);
                if (Weapon.IsReady && Weapon.CheckAction(Direction * xDir))
                    ExecuteAttackAction();
            }
        }

        private bool InDistance =>
            _maxDistance > 0 ? Vector3.Magnitude(currentPlayerPosition - view.Position) <= _maxDistance : true;
    }
}
