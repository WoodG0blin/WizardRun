using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Portal : LevelObject
    {
        public Portal(Vector2Int positionOnElement) : base("Portal", positionOnElement) { }

        protected override void OnInitiateView(GameObject gameObject) =>
            gameObject.AddComponent<PortalView>().Init(OnTrigger);

        private void OnTrigger(IInteractive view)
        {
            Debug.Log("Level FINISHED");
            //TODO - endGame
        }
    }
}
