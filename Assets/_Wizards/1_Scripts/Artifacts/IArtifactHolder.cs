using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifactHolder
    {
        CharacterStats Stats { get; }
        ActionsHolder Actions { get; }
        bool IsPlayer { get; }
    }

    public interface IArtifactUser : IArtifactHolder
    {
        Vector2 Direction { get; }
        IArtifactExecutor Weapon { get; }
        IViewMover Mover { get; }
        Coroutine SetTimer(float time, Action<float> informOnRemainingTime, Coroutine toStop = null);
        Transform Barrel { get; }
        void ResetMover(Func<Transform, IViewMover> setter);
    }
}
