using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Fireplace : ActiveObject
    {
        private FireplaceView _fireplaceView;
        public Fireplace(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            isPlayer = false;
            weapon = new Artifact(config.WeaponConfig).GetExecutor(ArtifactExecutorType.Attack);
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<FireplaceView>();

        protected override void OnInitiateView()
        {
            _fireplaceView = view as FireplaceView;
            AmmoView ammo = config.WeaponConfig.Ammo.GetComponent<AmmoView>();
            _fireplaceView.Init(config.WeaponConfig.ActionDistance, config.WeaponConfig.FireForce, ammo.Mass, ammo.Gravity);
            Barrel = _fireplaceView.Barrel;
            _fireplaceView.OnFireReady = Fire;
        }

        private void Fire(Vector3 direction)
        {
            Direction = direction;
            weapon.Use(this);
        }

        public override void SetNewPlayerPosition(Vector3 playerPosition)
        {
            _fireplaceView.SetNewPlayerPosition(playerPosition);
        }
    }
}
