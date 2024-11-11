using UnityEngine;

namespace WizardsPlatformer
{
    internal class Enemy : ActiveObject
    {
        private DemonView _demonView;

        public Enemy(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            isPlayer = false;
            weaponArtifact = new Artifact(config.WeaponConfig);
            weapon = weaponArtifact.GetExecutor(ArtifactExecutorType.Attack);
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<DemonView>();

        protected override void OnInitiateView()
        {
            _demonView = view as DemonView;
            _demonView.Init(config, Fire, () => weaponArtifact.IsReady);
            Barrel = _demonView.Barrel;
        }

        public void Fire(Vector3 direction)
        {
            Direction = direction;
            weapon.Use(this);
            _demonView.Fire(direction);
        }

        public override void SetNewPlayerPosition(Vector3 playerPosition)
        {
            _demonView.SetNewPlayerPosition(playerPosition);
        }
    }
}
