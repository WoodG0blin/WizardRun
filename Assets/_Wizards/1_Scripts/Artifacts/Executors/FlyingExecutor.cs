using UnityEngine;

namespace WizardsPlatformer
{
    public class FlyingExecutor : ArtifactPropertyExecutor
    {
        public FlyingExecutor(ArtifactProperty parent) : base(parent) { }
        public override void Use(IArtifactUser holder)
        {
            Debug.Log($"Set to flying");
            holder.ResetMover(t => new FlyingViewMover(t));
            holder.SetTimer(parentProperty.ActionValue, time => { if (time <= 0) holder.ResetMover(t => new ViewMover(t)); });
        }
    }
}