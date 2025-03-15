using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerView : LevelObjectView
    {
        [SerializeField] private Transform _barrel;


        public void DisplayHit() => animator.TriggerAnimation(ActionState.Hurt);

        public void DisplayDying() => animator.TriggerAnimation(ActionState.Die);

        public void DisplayAttack(Action onAttackPositionReady) => animator.TriggerAnimation(ActionState.Attack, onAttackPositionReady);


        protected override void OnInitiation()
        {
            Mover = new ViewMover(visualBody);
            Jumper = Mover as IJump;
        }

        protected override void OnUpdate()
        {
            animator.UpdateValues(Mover.Velocity);
        }


        protected override void OnCollision(IInteractionResponder interactor)
        {
            if (!interactor.IsPlayer)
            {
                OnInteraction?.Invoke(interactor);
            }
        }
        public Transform GetBarrelObject() { return _barrel ?? visualBody.Find("Barrel"); }
    }
}
