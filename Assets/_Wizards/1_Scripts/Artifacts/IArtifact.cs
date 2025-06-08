using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifact : IItem
    {
        List<ICharacterModifier> PassiveCharacterModifiers { get; }

        void SetHolder(IArtifactHolder holder);
        void TryUseFor(Artifact.ArtifactActivatorTypes actionType);
    }
}
