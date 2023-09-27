using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class JumpPlatformView : View, ILevelObjectView
    {
        public void Draw(Vector3 position)
        {
            SetPosition(position);
            SetActive(true);
        }
    }
}