using UnityEngine;

namespace WizardsPlatformer
{
    public class FlyingExecutor : ArtifactPropertyExecutor
    {
        private MovementType _initialMovement = MovementType.None;
        public FlyingExecutor(ArtifactProperty parent) : base(parent) { }
        public override void Use(IArtifactUser holder)
        {
            if (holder.Mover != null)
            {
                _initialMovement = holder.Mover.Type;
                Debug.Log($"Set to flying");
                holder.Mover.OnRequestReset?.Invoke(MovementType.Flying);
                holder.SetTimer(parentProperty.ActionValue, time => { if (time <= 0) holder.Mover.OnRequestReset?.Invoke(_initialMovement); });
            }
        }
    }
}