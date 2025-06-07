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
            _maxDistance = config.WeaponConfig.ActionDistance;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<DirectShooterView>();

        protected override void OnInitiateView()
        {
            view = base.view as DirectShooterView;
            view.Init();
            view.SetUpdateActions(UpdateAim);
            
            barrel = view.Barrel;

            base.OnInitiateView();
        }


        private void UpdateAim()
        {
            if (InDistance)
            {
                Direction = view.RotateBarrelTowards(currentPlayerPosition);
                if (weaponArtifact.IsReady) weaponArtifact.Fire(Direction);
            }
        }

        private bool InDistance =>
            _maxDistance > 0 ? Vector3.Magnitude(currentPlayerPosition - view.Position) <= _maxDistance : true;
    }
}
