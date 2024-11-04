using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Portal : LevelObject
    {
        public Portal(Vector2Int positionOnElement) : base("Portal", positionOnElement) { }

        protected override void OnInitiateView(GameObject gameObject) =>
            gameObject.AddComponent<PortalView>();

    }
}
