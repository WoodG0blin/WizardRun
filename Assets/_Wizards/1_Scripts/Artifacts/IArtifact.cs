using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifact
    {
        string Name { get; }
        Sprite Icon { get; }
        Sprite LevelView { get; }
        ArtifactSlotType SlotType { get; }
        List<ICharacterModifier> PassiveCharacterModifiers { get; }
        bool HasPassiveArtifactModifier { get; }
        bool IsActive { get; }
    }
}
