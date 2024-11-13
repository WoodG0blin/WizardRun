using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class FireplaceView : LevelObjectView
    {
        [field: SerializeField] public Transform Barrel { get; private set; }
        public BallisticAimView Aim { get; private set; }

        public Action<Vector3> OnFireReady { get; set; }

        public void Init(float actionDistance, float fireForce, float ammoMass, float gravityCoefficient)
        {
            Barrel ??= transform.Find("Aim");

            if (Barrel == null) return;

            if (!Barrel.TryGetComponent<BallisticAimView>(out var aim)) aim = Barrel.gameObject.AddComponent<BallisticAimView>();
            Aim = aim;

            Aim.Init(actionDistance, fireForce/ammoMass, gravityCoefficient);
        }

        protected override void OnUpdate()
        {
            if (Aim.InDistance) OnFireReady?.Invoke(Aim.Direction);
        }

        public void SetNewPlayerPosition(Vector3 newPlayerPosition)
        {
            Aim?.UpdateAim(newPlayerPosition);
        }
        protected override void OnCollision(IInteractionResponder interactor)
        {
            if (interactor.IsPlayer)
            {
                OnInteraction?.Invoke(interactor);
            }
        }
    }
}