using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Enemy : LevelObject
    {
        public Enemy(Vector2Int gridPosition) : base("Demon", gridPosition) { }

        protected override void OnInitiateView(GameObject gameObject)
        {
            gameObject.AddComponent<DemonView>().Init(config);
        }
    }
}
