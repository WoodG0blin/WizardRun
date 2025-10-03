using UnityEngine;

namespace WizardsPlatformer
{
    public class ExtraJumpExecutor : ArtifactPropertyExecutor
    {
        private int _counter = 0;
        public ExtraJumpExecutor(ArtifactProperty parent) : base(parent) { }

        public override void Use(IArtifactUser holder)
        {
            Debug.Log($"Using extra jump");
            if (holder.Mover.IsGrounded) _counter = 0;
            else
            {
                if(_counter < parentProperty.ActionValue && !holder.Mover.IsExecutingJump)
                {
                    holder.Mover.Jump(holder.Stats.JumpForce);
                    _counter++;
                }
            }
        }
    }
}