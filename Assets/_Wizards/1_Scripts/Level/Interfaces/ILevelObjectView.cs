using UnityEngine;

namespace WizardsPlatformer
{
    internal interface ILevelObjectView
    {
        void Draw(Vector3 position);
        void FinishInitiation();
    }
}