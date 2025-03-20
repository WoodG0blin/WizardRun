using System;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class BowlView : LevelObjectView
    {
        [field: SerializeField] public Transform Barrel { get; private set; }
        public AimView Aim { get; private set; }

        public Action<Vector3> OnFireReady { get; set; }

        public void Init(float actionDistance)
        {
            Barrel ??= visualBody.Find("Aim");

            if (Barrel == null) return;

            if (!Barrel.TryGetComponent<AimView>(out var aim)) aim = Barrel.gameObject.AddComponent<AimView>();
            Aim = aim;

            Aim.Init(actionDistance);
        }

        protected override void OnUpdate()
        {
            if (Aim.InDistance) OnFireReady?.Invoke(Aim.Direction);
        }

        public void SetNewPlayerPosition(Vector3 newPlayerPosition)
        {
            Aim.UpdateAim(newPlayerPosition);
        }
    }
}