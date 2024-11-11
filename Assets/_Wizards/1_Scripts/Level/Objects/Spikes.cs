using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class Spikes : InteractableObject
    {
        //TODO replace with config load?
        int _damage = 20;

        public Spikes(LevelObjectConfig config, Vector2Int gridPosition) : base(config, gridPosition) { }

        protected override LevelObjectView SetView(GameObject gameObject) =>
            gameObject.AddComponent<SpikesView>();

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            Debug.Log($"interaction with spikes. Interactor player? {interactor.IsPlayer}");
            if (interactor.IsPlayer) interactor.ReceiveDamage(_damage);
        }
    }
}
