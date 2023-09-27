using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Bowl : LevelObject
    {
        public Bowl(Vector2Int gridPosition) : base("Bowl", gridPosition) { }

        protected override void OnInitiateView(GameObject gameObject)
        {
            gameObject.AddComponent<BowlView>().Init(config);
        }

    }
}
