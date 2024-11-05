using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifactHolder
    {
        bool IsPlayer { get; }
        List<IArtifact> EquippedArtifacts { get; }
        Transform Barrel { get; }
        UnityEngine.Vector3 Direction { get; }
        IJump JumpExecutioner { get; }
    }
}
