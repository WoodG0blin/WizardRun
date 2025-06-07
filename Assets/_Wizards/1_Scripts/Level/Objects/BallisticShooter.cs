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
        private float _maxDistance;

        public BallisticShooter(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition)
        {
            //weaponArtifact = new Weapon(config.WeaponConfig);
            //weapon = weaponArtifact.GetExecutor(ArtifactExecutorType.Attack);
            _maxDistance = config.WeaponConfig.ActionDistance;
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<BallisticShooterView>();

        protected override void OnInitiateView()
        {
            view = base.view as BallisticShooterView;

            //AmmoView ammo = config.WeaponConfig.Ammo.GetComponent<AmmoView>();
            view.Init(weaponArtifact.FireForce);
            view.SetUpdateActions(UpdateAim);

            barrel = view.Barrel;

            base.OnInitiateView();
        }

        private void UpdateAim()
        {
            if (InDistance)
            {
                Direction = view.RotateBarrelTowards(currentPlayerPosition);
                //Debug.Log($"Setting ballistic direction to {Direction}. Weapon ready? {weaponArtifact.IsReady}");
                if (weaponArtifact.IsReady)
                {
                    weaponArtifact.Fire(Direction);
                }
            }
        }

        private bool InDistance =>
            _maxDistance > 0 ? Vector3.Magnitude(currentPlayerPosition - view.Position) <= _maxDistance : true;

    }
}
