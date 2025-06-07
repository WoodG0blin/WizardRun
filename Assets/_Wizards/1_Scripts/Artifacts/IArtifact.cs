using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifact : IDisplayInfo
    {
        ArtifactSlotType SlotType { get; }
        List<ICharacterModifier> PassiveCharacterModifiers { get; }
        IArtifactExecutor GetExecutor(ArtifactExecutorType type);

        void SetHolder(IArtifactHolder holder);
        void TryUseFor(Artifact.ExecutorType actionType);

        bool IsReady { get; }
    }
}
