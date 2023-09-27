using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Spikes : LevelObject
    {
        //TODO replace with config load?
        float _damage = 20f;

        public Spikes(Vector2Int gridPosition) : base("Spikes", gridPosition) { }
        protected override void OnInitiateView(GameObject gameObject)
        {
            gameObject.AddComponent<SpikesView>().Init(_damage);
        }
    }
}
