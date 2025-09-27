using System.Collections.Generic;
using UnityEngine;
using static WizardsPlatformer.Artifact;

namespace WizardsPlatformer
{
    public interface IArtifact : IItem
    {
        void Equip(IArtifactHolder holder);
        void Unequip();
        //List<ICharacterModifier> PassiveCharacterModifiers { get; }

        //void SetHolder(IArtifactHolder holder);
        //void TryUseFor(Artifact.ArtifactActivatorTypes actionType);
    }

    //internal interface IModifiableArtifact
    //{
    //    public void SetModifiers(List<IArtifactModifier> modifiers);
    //    public void ClearAllModifiers();
    //    public ArtifactActor GetActor(ArtifactActivatorTypes type);
    //}

}
