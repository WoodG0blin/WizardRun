using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Bowl : ActiveObject, IArtifactHolder
    {
        private BowlView _bowlView;
        public Bowl(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            isPlayer = false;
            weapon = new Artifact(config.WeaponConfig).GetExecutor(ArtifactExecutorType.Attack);
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<BowlView>();

        protected override void OnInitiateView()
        {
            _bowlView = view as BowlView;
            _bowlView.Init(config.WeaponConfig.ActionDistance);
            Barrel = _bowlView.Barrel;
            _bowlView.OnFireReady = Fire;

            base.OnInitiateView();
        }

        private void Fire(Vector3 direction)
        {
            Direction = direction;
            weapon.Use(this);
        }

        protected override void SetNewPlayerPosition(Vector3 playerPosition)
        {
            base.SetNewPlayerPosition(playerPosition);
            _bowlView.SetNewPlayerPosition(playerPosition);
        }
    }
}
