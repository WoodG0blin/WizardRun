using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifactHolder
    {
        CharacterStats Stats { get; }
        bool IsPlayer { get; }
        List<IArtifact> EquippedArtifacts { get; }
        IInteractionResponder GetTargetAt(float distance);
        Vector2 Direction { get; }
        IJump JumpExecutioner { get; }
        Coroutine SetTimer(float time, Action<float> informOnRemainingTime, Coroutine toStop = null);
        internal void PlaceAmmo(LevelObject ammo);
    }
}
