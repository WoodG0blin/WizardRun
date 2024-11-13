using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IArtifact : IDisplayInfo
    {
        ArtifactSlotType SlotType { get; }
        List<ICharacterModifier> PassiveCharacterModifiers { get; }
        IArtifactExecutor GetExecutor(ArtifactExecutorType type);
        bool IsReady { get; }

    }

    internal interface IArtifactExecutorHolder
    {
        bool IsReady { get; }
        int Damage { get; }
        float ActionDistance { get; }
        float FireForce { get; }
        bool TryGetAmmoTo(Transform barrel, out AmmoView ammo);
        void Activate();
    }
}
