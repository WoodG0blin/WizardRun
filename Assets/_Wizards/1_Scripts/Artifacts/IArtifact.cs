using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifact : IDisplayInfo
    {
        ArtifactSlotType SlotType { get; }
        List<ICharacterModifier> PassiveCharacterModifiers { get; }
        IArtifactExecutor GetExecutor(ArtifactExecutorType type);
    }

    internal interface IArtifactExecutorHolder
    {
        bool IsReady { get; }
        int Damage { get; }
        int ActionDistance { get; }
        int FireForce { get; }
        bool TryGetAmmoTo(Transform barrel, out AmmoView ammo);
        void Activate();
    }
}
