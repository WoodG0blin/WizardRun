using UnityEngine;

namespace WizardsPlatformer
{
    internal class BonusView : LevelObjectView
    {
        private AnimationController _animator;

        protected override void OnCollision(IInteractionResponder interactor)
        {
            if(interactor.IsPlayer)
            {
                OnInteraction?.Invoke(interactor);
                SetActive(false);
            }
        }
    }
}