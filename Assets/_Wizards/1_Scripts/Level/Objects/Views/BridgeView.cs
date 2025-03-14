using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class BridgeView : LevelObjectView
    {
        public void Init(float angle) =>
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
    }
}