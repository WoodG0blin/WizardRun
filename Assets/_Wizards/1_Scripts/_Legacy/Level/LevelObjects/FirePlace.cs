using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Fireplace : LevelObject
    {
        public Fireplace(Vector2Int gridPosition) : base("Fireplace", gridPosition) { }

        protected override void OnInitiateView(GameObject gameObject)
        {
            gameObject.AddComponent<FireplaceView>().Init(config);
        }
    }
}
