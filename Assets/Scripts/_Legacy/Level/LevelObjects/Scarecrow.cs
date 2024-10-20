using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Scarecrow : LevelObject
    {
        private float _targetCooldown = 2f;
        private float _force = 1.2f;

        public Scarecrow(Vector2Int _gridPosition) : base("Scarecrow", _gridPosition) { }

        protected override void OnInitiateView(GameObject gameObject)
        {
            gameObject.AddComponent<ScarecrowView>().Init(config);
        }
    }
}
