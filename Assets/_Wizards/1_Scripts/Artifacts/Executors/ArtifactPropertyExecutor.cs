using UnityEngine;

namespace WizardsPlatformer
{
    public class ArtifactPropertyExecutor
    {
        protected ArtifactProperty parentProperty;

        public ArtifactPropertyExecutor(ArtifactProperty parent)
        {
            parentProperty = parent;
        }

        public virtual void Init(IArtifactHolder holder) { }

        public virtual void Use(IArtifactUser holder)
        {
            UnityEngine.Debug.Log($"Executing Stub for {parentProperty.NameTag}");
        }

        public virtual bool CheckAction(Vector2 relativeTarget) => true;
    }
}