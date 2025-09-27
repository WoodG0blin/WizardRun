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

        //bool IsPlayer { get; }
        //IInteractionResponder GetTargetAt(float distance);
        //Vector2 Direction { get; }
        //IJump JumpExecutioner { get; }
        //Coroutine SetTimer(float time, Action<float> informOnRemainingTime, Coroutine toStop = null);
        //internal void PlaceAmmo(LevelObject ammo);
    }

    public interface IArtifactUser : IArtifactHolder
    {
        IInteractionResponder GetTargetAt(float distance);
        Vector2 Direction { get; }
        IJump JumpExecutioner { get; }
        Coroutine SetTimer(float time, Action<float> informOnRemainingTime, Coroutine toStop = null);
        Transform Barrel { get; }
        internal void PlaceAmmo(LevelObject ammo);
    }
}
