using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifactHolder
    {
        public CharacterStats Stats { get; }
        bool IsPlayer { get; }
        List<IArtifact> EquippedArtifacts { get; }
        Transform Barrel { get; }
        UnityEngine.Vector3 Direction { get; }
        IJump JumpExecutioner { get; }
    }
}
