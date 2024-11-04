using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IJump : IView
    {
        void Jump(float force);
    }
}