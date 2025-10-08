using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerView : LevelObjectView
    {
        [SerializeField] private Transform _barrel;
        [SerializeField] private LevelObjectView _portal;

        public void DisplayHit() => animator.TriggerAnimation(ActionState.Hurt);

        public void DisplayDying() => animator.TriggerAnimation(ActionState.Die);

        public void DisplayAttack(Action onAttackPositionReady) => animator.TriggerAnimation(ActionState.Attack, onAttackPositionReady);


        protected override void OnInitiation()
        {
            Mover = new ViewMover(visualBody);
        }

        protected override void OnUpdate()
        {
            animator.UpdateValues(Mover.Velocity);
        }

        public Transform GetBarrelObject() { return _barrel ?? visualBody.Find("Barrel"); }

        public void InitiatePortal(Action onEnter, Vector3 offset)
        {
            var port = GameObject.Instantiate(_portal, Position + offset, Quaternion.identity, null);
            port.SetActive(true);
            port.OnInteraction += interactor =>
            {
                if (interactor.IsPlayer)
                {
                    if (onEnter == null)
                        StartCoroutine(ClosePortal(port));
                    else
                    {
                        port.SetActive(false);
                        onEnter.Invoke();
                    }
                }
            };
        }

        private IEnumerator ClosePortal(LevelObjectView port)
        {
            yield return new WaitForSeconds(1);
            port.SetActive(false);
        }
    }
}
