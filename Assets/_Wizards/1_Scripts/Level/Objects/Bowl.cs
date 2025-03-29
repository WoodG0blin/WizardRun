using UnityEngine;

namespace WizardsPlatformer
{
    internal class Bowl : ActiveObject, IArtifactHolder
    {
        private new BowlView view;
        private float _maxDistance;

        public Bowl(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            weaponArtifact = new Artifact(config.WeaponConfig);
            weapon = weaponArtifact.GetExecutor(ArtifactExecutorType.Attack);
            _maxDistance = config.WeaponConfig.ActionDistance;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<BowlView>();

        protected override void OnInitiateView()
        {
            view = base.view as BowlView;
            view.Init();
            view.SetUpdateActions(UpdateAim);
            
            Barrel = view.Barrel;

            base.OnInitiateView();
        }


        private void UpdateAim()
        {
            if (InDistance)
            {
                Direction = view.RotateBarrelTowards(currentPlayerPosition);
                if (weaponArtifact.IsReady) weapon.Use(this);
            }
        }

        private bool InDistance =>
            _maxDistance > 0 ? Vector3.Magnitude(currentPlayerPosition - view.Position) <= _maxDistance : true;
    }
}
