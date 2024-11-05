using System.Collections.Generic;

namespace WizardsPlatformer
{
    internal interface IModifiableArtifact
    {
        public void SetModifiers(List<IArtifactModifier> modifiers);
        public void ClearAllModifiers();
    }
}
