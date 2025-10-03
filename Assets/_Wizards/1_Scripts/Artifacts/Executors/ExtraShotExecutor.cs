using UnityEngine;

namespace WizardsPlatformer
{
    public class ExtraShotExecutor : ArtifactPropertyExecutor
    {
        private int _counter = 0;
        public ExtraShotExecutor(ArtifactProperty parent) : base(parent) { }

        public override void Use(IArtifactUser holder)
        {
            Debug.Log($"Using extra shot");

            _counter = 0;
            if (_counter < parentProperty.ActionValue)
            {
                holder.Weapon.Use(holder);
                _counter++;
            }
        }
    }
}