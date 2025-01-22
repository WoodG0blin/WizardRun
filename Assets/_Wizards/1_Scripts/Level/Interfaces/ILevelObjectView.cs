using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface ILevelObjectView
    {
        void Draw(Vector3 position);
        void SetActive(bool active);

        Vector3 Position { get; }
        void MoveTo(Vector2 direction);
        void DisplayJump(float force);

        IContactsPuller Contacts { get; }

        void StartAttack(Action onLaunch);

        void FinishInitiation();
        void SetUpdateActions(Action onUpdate);
    }
}