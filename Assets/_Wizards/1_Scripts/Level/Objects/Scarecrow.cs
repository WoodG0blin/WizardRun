using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Scarecrow : ActiveObject
    {
        public Scarecrow(LevelObjectConfig config, Vector2Int _gridPosition) : base(config, _gridPosition)
        {
            //weapon = new Artifact(config.WeaponConfig).GetExecutor(ArtifactExecutorType.Attack);
        }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<ScarecrowView>();

        protected override void OnInitiateView()
        {
            ScarecrowView bv = view as ScarecrowView;
            bv.Init();
            barrel = bv.Barrel;
            bv.OnFireReady = Fire;

            base.OnInitiateView();
        }

        private void Fire(Vector3 direction)
        {
            Direction = direction;
            if (weaponArtifact.IsReady)
                weaponArtifact.Fire(Direction);
        }
    }
}
