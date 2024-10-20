using UnityEngine;

namespace WizardsPlatformer
{
    internal interface ILevelObjectView : IView
    {
        void Draw(Vector3 position);
    }
}