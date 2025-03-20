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
            gameObject.AddComponent<LevelObjectView>();

        protected override void ActionsOnInteraction(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                Debug.Log("player interaction with spikes");
                interactor.ReceiveDamage(_damage);
                interactor.KickOff(0.5f);
                view.SetActive(false);
            }
        }
    }
}
