using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class BridgeView : LevelObjectView
    {
        public void Init(float angle) =>
            SetRotation(Quaternion.AngleAxis(angle, Vector3.back));
    }
}